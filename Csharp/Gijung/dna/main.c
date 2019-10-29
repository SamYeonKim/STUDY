#define _CRT_SECURE_NO_WARNINGS

#include <stdlib.h>
#include <stdarg.h>
#include <stdio.h>
#include <inttypes.h>
#include <string.h>
#include <io.h>
#include <fcntl.h>

#pragma pack(1)

#define T_CALLOC(type) ((type*)calloc(1, sizeof(type)))
#define PTR(p, ofs) ((void*)((char*)(p) + (ofs)))
#define VAL(type, p, ofs) (*(type*)(PTR(p, ofs)))

void Crash(char* pMsg, ...) {
	printf("\n\n*** CRASH ***\n");
	va_list va;
	va_start(va, pMsg);
	vprintf(pMsg, va);
	va_end(va);
	printf("\n\n(Press enter to exit)\n");
	getchar();
	exit(1);
}

typedef struct tRVA tRVA;
struct tRVA {
	int baseAddress;	// 기본 주소
	int size;			// 크기
	void* pData;		// 데이터 주소
	tRVA* pNext;
};

typedef struct tMetaDataTable tMetaDataTable;
struct tMetaDataTable {
	void **ppData;
	int count;
};

#define TABLE_ID(token) ((token) >> 24)
#define TABLE_OFS(token) ((token) & 0x00ffffff)
#define MAKE_TOKEN(tableId, ofs) ((((int)(tableId)) << 24) | (((int)(ofs)) & 0x00ffffff))
#define GET_TABLE_ENTRY(type, pFile, token) (type*)(pFile->tables[TABLE_ID(token)].ppData[TABLE_OFS(token)])
#define GET_STRING(pFile, ofs) ((pFile)->pStrings + (ofs))

typedef struct tFile tFile;
struct tFile {
	tRVA* pRVAs;
	int entryPointToken;
	char *pStrings;
	char *pUserStrings;
	tMetaDataTable tables[64];
};

typedef struct tMD_Prefix tMD_Prefix;
struct tMD_Prefix {
	tFile *pFile;
};

#define TABLE_ID_METHODDEF 0x06
typedef struct tMD_MethodDef tMD_MethodDef;
struct tMD_MethodDef {
	tMD_Prefix prefix;
	int rva;
	short implFlags;
	short flags;
	short nameIndex; // Strings index
	short sigIndex; // Blob index
	short paramListIndex; // tMD_Param index
};

int md_tableSizes[64] = {
	10, 6, 14, 0, 6, 0, 14, 0, // 0x00 - 0x07
	6, 0, 6, 0, 6, 0, 0, 0,    // 0x08 - 0x0f
	0, 2, 0, 0, 0, 0, 0, 0,    // 0x10 - 0x17
	0, 0, 0, 0, 0, 0, 0, 0,    // 0x18 - 0x1f
	22, 0, 0, 20               // 0x20 - 0x23
};

// 상대 주소에 위치한 데이터 정보를 추출하는 함수
void* RVA_FindData(tFile *pFile, int addr) {
	tRVA *pRVA = pFile->pRVAs;
	while (pRVA != NULL) {
		if (addr >= pRVA->baseAddress && addr < pRVA->baseAddress + pRVA->size) {
			return PTR(pRVA->pData, addr - pRVA->baseAddress);
		}
		pRVA = pRVA->pNext;
	}
	return NULL;
}

// 파일로부터 데이터를 추출하는 함수
tFile* LoadFile(char *pFilename) {
	printf("Load file: '%s'\n", pFilename);

	// Load file
	int f = _open(pFilename, O_BINARY | O_RDONLY);	// 이진형태 | 읽기 전용으로 오픈한다.
	int len = _lseek(f, 0, SEEK_END);				// 파일의 끝(SEEK_END)으로 읽기 포인터를 옮겨 길이를 구한다.
	_lseek(f, 0, SEEK_SET);							// 파일의 시작(SEEK_SET)으로 읽기 포인터를 옮긴다.
	void *pData = malloc(len);						// 길이만큼 할당.
	_read(f, pData, len);							// 파일의 길이만큼 읽어들인다.
	_close(f);

	tFile *pFile = T_CALLOC(tFile);					// 구조체 할당 (calloc은 0으로 자동 초기화).

	if (VAL(char, pData, 0) != 'M' || VAL(char, pData, 1) != 'Z') Crash("Not an executable!");	// DOS_HEADER의 SIGNATURE를 읽어 제작자 MZ가 맞는지 확인한다.
	printf("Is an executable :)\n");
	void *pMSDOSHeader = pData;
	int lfanew = VAL(int, pMSDOSHeader, 0x3c);				// IMAGE_NT_HEADERS의 주소값 저장한다.
	void *pPEHeader = PTR(pMSDOSHeader, lfanew + 4);		// IMAGE_NT_HEADERS의 IMAGE_FILE_HEADER 주소값 저장한다.
	if (VAL(short, pPEHeader, 0) != 0x14c) Crash("This is not a .NET executable!");
	printf("It is a .NET executable :)\n");
	void *pPEOptionalHeader = PTR(pPEHeader, 20);			// IMAGE_NT_HEADERS의 IMAGE_OPTIONAL_HEADER 주소값 저장한다.
	void *pSectionHeaders = PTR(pPEOptionalHeader, 224);	// IMAGE_SECTION_HEADER의 주소값 저장한다.
	int numSections = VAL(short, pPEHeader, 2);				// Section의 개수를 읽어온다.
	printf("Number of sections: %i\n", numSections);
	for (int i = 0; i < numSections; i++) {					// Section의 정보를 구성한다.
		void *pSection = PTR(pSectionHeaders, i * 40);
		tRVA *pRVA = T_CALLOC(tRVA);
		pRVA->baseAddress = VAL(int, pSection, 12);
		pRVA->size = VAL(int, pSection, 8);
		pRVA->pData = calloc(1, pRVA->size);
		int rvaOfs = VAL(int, pSection, 20);
		memcpy(pRVA->pData, PTR(pData, rvaOfs), min(pRVA->size, VAL(int, pSection, 16)));
		pRVA->pNext = pFile->pRVAs;
		pFile->pRVAs = pRVA;
	}
	printf("Loaded sections :)\n");

	// Load CLI header
	void *pCLIHeader = RVA_FindData(pFile, VAL(int, pPEOptionalHeader, 208));	// IMAGE_OPTIONAL_HEADER에 저장되어있는 CLI Header의 주소값을 저장한다.
	printf("Runtime version: %i.%i\n", VAL(short, pCLIHeader, 4), VAL(short, pCLIHeader, 6));
	pFile->entryPointToken = VAL(int, pCLIHeader, 20);							// CLI 시작 주소를 저장한다.
	printf("entry-point token: 0x%08x\n", pFile->entryPointToken);
	void *pMetaData = RVA_FindData(pFile, VAL(int, pCLIHeader, 8));
	printf("CLI version: '%s'\n", (char*)PTR(pMetaData, 16));
	int versionLen = VAL(int, pMetaData, 12);
	int ofs = 16 + versionLen;
	int StreamCount = VAL(short, pMetaData, ofs + 2);
	printf("Metadata stream count: %i\n", StreamCount);
	ofs += 4;
	for (int i = 0; i < StreamCount; i++) {
		int streamOffset = VAL(int, pMetaData, ofs);
		int streamSize = VAL(int, pMetaData, ofs + 4);
		char *pStreamName = PTR(pMetaData, ofs + 8);
		void *pStream = PTR(pMetaData, streamOffset);
		ofs += 8 + ((strlen(pStreamName) + 4) & (~3));
		printf("Stream found: '%s'\n", pStreamName);
		if (strcmp(pStreamName, "#Strings") == 0) {
			pFile->pStrings = pStream;
		}
		if (strcmp(pStreamName, "#~") == 0) {
			// Load tables
			long long valid = VAL(long long, pStream, 8);
			int *pRowCounterPtr = PTR(pStream, 24);
			for (long long i = 0, j = 1; i < 64; i++, j <<= 1) {
				if (valid & j) {
					pFile->tables[i].count = *pRowCounterPtr;
					pRowCounterPtr += 1;
				}
			}
			// Load data
			void *pTableData = pRowCounterPtr;
			printf("Loaded tables:\n");
			for (int i = 0; i < 64; i++) {
				int count = pFile->tables[i].count;
				if (count != 0) {
					int tableSize = md_tableSizes[i];
					void **ppData = pFile->tables[i].ppData = malloc(sizeof(void*) * (count + 1));
					ppData[0] = NULL;
					for (int j = 0; j < count; j++) {
						tMD_Prefix *pPrefix = malloc(sizeof(tMD_Prefix) + tableSize);
						pPrefix->pFile = pFile;
						memcpy(pPrefix + 1, pTableData, tableSize);
						ppData[j + 1] = pPrefix;
						pTableData = PTR(pTableData, tableSize);
					}
					printf("  Table 0x%02x: %i entries\n", i, count);
				}
			}
		}
	}
	return pFile;
}

// 파일로 부터 함수를 추출하는 함수
tMD_MethodDef* GetMethodByToken(tFile *pFile, int methodToken) {
	switch (TABLE_ID(methodToken)) {
	case TABLE_ID_METHODDEF:
		return GET_TABLE_ENTRY(tMD_MethodDef, pFile, methodToken);
	}
	Crash("!!!!");
	return NULL;
}

typedef struct tMethodState tMethodState;
struct tMethodState {
	tFile *pFile;				// 파일 주소
	tMD_MethodDef *pMethodDef;	// 시작 함수 주소
	void *pIL;					// IL 주소
	int ip;
	char evalStack[32];			// 스택값 저장
	int esp;
};

// 함수 구조체를 할당하는 함수
tMethodState* CreateMethodState(tMD_MethodDef *pMethodDef) {
	tMethodState *pMethodState = T_CALLOC(tMethodState);
	pMethodState->pFile = pMethodDef->prefix.pFile;
	pMethodState->pMethodDef = pMethodDef;
	void *pILHeader = RVA_FindData(pMethodDef->prefix.pFile, pMethodDef->rva);
	if ((VAL(char, pILHeader, 0) & 0x3) != 0x2) Crash("Can only understand Tiny IL Headers");
	pMethodState->pIL = PTR(pILHeader, 1);
	return pMethodState;
}

// ILCode 실행함수
void Execute(tMethodState *pMethodState) {
	tFile *pFile = pMethodState->pFile;
	printf("\nExecuting method: '%s'\n", GET_STRING(pFile, pMethodState->pMethodDef->nameIndex));
	void *pIL = pMethodState->pIL;
	void *pEvalStack = pMethodState->evalStack;
	for (;;) {
		unsigned char opcode = VAL(unsigned char, pIL, pMethodState->ip);
		pMethodState->ip += 1;
		printf("Executing opcode: 0x%02x\n", opcode);
		switch (opcode) {
		case 0x1f: // LDC.I4.S
		{
			// Load int32 from short-form (signed byte).
			int value = VAL(char, pIL, pMethodState->ip);
			pMethodState->ip += 1;
			// Store in top of evaluation stack.
			VAL(int, pEvalStack, pMethodState->esp) = value;
			pMethodState->esp += 4;
			break;
		}
		case 0x28: // CALL
		{
			// Load method-def|ref token from IL.
			// Only method-defs are handle currently. I.e. the method being called must in this assembly.
			int callToken = VAL(int, pIL, pMethodState->ip);
			pMethodState->ip += 4;
			// Load the method-definition from the metadata, and create a method state.
			tMD_MethodDef *pCallMethodDef = GetMethodByToken(pFile, callToken);
			tMethodState *pCallMethodState = CreateMethodState(pCallMethodDef);
			// Execute the method.
			Execute(pCallMethodState);
			// If it has a return-value, copy it to to evaluation-stack of this method.
			memcpy(PTR(pEvalStack, pMethodState->esp), pCallMethodState->evalStack, pCallMethodState->esp);
			pMethodState->esp += pCallMethodState->esp;
			break;
		}
		case 0x2a: // RET
			printf("Executing method return from: '%s' \n\n", GET_STRING(pFile, pMethodState->pMethodDef->nameIndex));
			return;
		case 0x58: // ADD
		{
			// Assume we're adding int32 values.
			// Read the top two int32 values from the evaluation stack, add them, then push back on to the evaluation stack.
			int value2 = VAL(int, pEvalStack, pMethodState->esp - 4);
			int value1 = VAL(int, pEvalStack, pMethodState->esp - 8);
			VAL(int, pEvalStack, pMethodState->esp - 8) = value1 + value2;
			pMethodState->esp -= 4;
			break;
		}
		default:
			Crash("Cannot (yet) execute opcode: 0x%02x\n", opcode);
		}
	}
}

int main(int argc, char** argp) {
	// 파일에서 CLI 정보 추출
	tFile *pFile = LoadFile(argp[1]);

	// CLI 정보에서 시작 함수 정보를 추출
	tMD_MethodDef *pEntryPointMethodDef = GetMethodByToken(pFile, pFile->entryPointToken);
	printf("Entry-point method name: '%s'\n", GET_STRING(pFile, pEntryPointMethodDef->nameIndex));

	// 실제 사용할 구조체 할당
	tMethodState *pMethodState = CreateMethodState(pEntryPointMethodDef);

	// 함수 실행
	Execute(pMethodState);

	int exitCode;
	if (pMethodState->esp == 4)
	{
		// If an int32 value is left at position 0 in the evaluation stack, then this is the exit-code.
		exitCode = VAL(int, pMethodState->evalStack, 0);
		printf("Execution completed successfully. Exit-code: %i", exitCode);
	}
	else
	{
		exitCode = 0;
		printf("Execution completed successfully. No exit-code");
	}

	printf("\n\n(Press enter to exit)\n");
	getchar();

	return exitCode;
}

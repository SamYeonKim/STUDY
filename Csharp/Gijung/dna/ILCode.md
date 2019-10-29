# ILCode

```c
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.0.30319.33440
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 

// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly i5xyehxb
{
  .hash algorithm 0x00008004
  .ver 0:0:0:0
}
.module i5xyehxb.dll
// MVID: {6CF9CDF2-4E58-417D-B696-936F2E9B7F88}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x015D0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit HelloWorld.Program
       extends [mscorlib]System.Object
{
  .method private hidebysig static int32 
          GetNumber() cil managed
  {
    // 
    .maxstack  1
    .locals init (int32 V_0)
    IL_0000:  nop
    IL_0001:  ldc.i4.s   84         // 값(84)을 스택에 Push.
    IL_0003:  stloc.0               // 스택에서 값을 pop 하여 0번 지역 변수에 저장.
    IL_0004:  br.s       IL_0006    // IL_0006 으로 제어권을 넘긴다는 의미.

    IL_0006:  ldloc.0               // 0번 인덱스에 있는 지역변수를 스택에 Push.
    IL_0007:  ret                   // 값 반한.
  } // end of method Program::GetNumber

  .method public hidebysig static int32  Main(string[] args) cil managed
  {
    // 
    .maxstack  2
    .locals init (int32 V_0)
    IL_0000:  nop
    IL_0001:  call       int32 HelloWorld.Program::GetNumber()  // 함수 호출.
    IL_0006:  ldc.i4.s   42         // 값(42)을 스택에 Push.
    IL_0008:  add                   // 두개의 값을 더하여 스택에 Push.
    IL_0009:  stloc.0
    IL_000a:  br.s       IL_000c

    IL_000c:  ldloc.0
    IL_000d:  ret
  } // end of method Program::Main

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method Program::.ctor

} // end of class HelloWorld.Program


// =============================================================

// 
// 
```
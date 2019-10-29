# JvmGo

## 진입점

```go
func main() {
	cmd, err := parseCommand(os.Args)
	if err != nil {
		fmt.Println(err)
		printUsage()
	} else {
		startJVM(cmd)
	}
}

func startJVM(cmd Command) {
	Xcpuprofile := cmd.Options.Xcpuprofile
	if Xcpuprofile != "" {
		f, err := os.Create(Xcpuprofile)
		if err != nil {
			panic(err)
		}
		pprof.StartCPUProfile(f)
		defer pprof.StopCPUProfile()
	}

	options.InitOptions(cmd.Options.VerboseClass, cmd.Options.Xss, cmd.Options.XuseJavaHome)
    // 기본 내장 클래스 로더 3종의 경로 지정
    cp := classpath.Parse(cmd.Options.Classpath)
    // BootStrap 클래스 로더 초기화
	heap.InitBootLoader(cp)

    mainClassName := jutil.ReplaceAll(cmd.Class, ".", "/")
    // 메인 스레드 실행
	mainThread := createMainThread(mainClassName, cmd.Args)
	interpreter.Loop(mainThread)
	interpreter.KeepAlive()
}
```

## 명령어 페치 및 실행

```go
func _loop(thread *rtda.Thread) {
	defer _catchErr(thread)

	for {
        // 현재 스택에 있는 프레임 가져오고 실행해야 하는 명령어 갱신
		frame := thread.CurrentFrame()
		pc := frame.NextPC()
		thread.SetPC(pc)

		// 현재 pc가 가리키는 명령어 가져옴
		method := frame.Method()
		if method.Instructions == nil {
			method.Instructions = decodeMethod(method.Code())
		}
		insts := method.Instructions.([]base.Instruction)
		inst := insts[pc]
		instCount := len(insts)

		// pc 갱신
		for {
			pc++
			if pc >= instCount || insts[pc] != nil {
				break
			}
		}
		frame.SetNextPC(pc)

		// 명령어 실행
		inst.Execute(frame)
		if thread.IsStackEmpty() {
			break
		}
	}
}
```

## 명령어

```go
func execMain(thread *rtda.Thread) {
	thread.PopFrame()
	mainClass := _classLoader.LoadClass(_mainClassName)
	mainMethod := mainClass.GetMainMethod()
	if mainMethod != nil {
        // jvm 스택에 프레임 만들어서 쌓는다.
		newFrame := thread.NewFrame(mainMethod)
		thread.PushFrame(newFrame)
		args := createArgs()
		newFrame.LocalVars().SetRef(0, args)
	} else {
		panic("no main method!") // todo
	}
}

func (self *NEW) Execute(frame *rtda.Frame) {
	if self.class == nil {
		cp := frame.ConstantPool()
		kClass := cp.GetConstant(self.Index).(*heap.ConstantClass)
		self.class = kClass.Class()
	}

	// init class
	if self.class.InitializationNotStarted() {
		frame.RevertNextPC()
		frame.Thread().InitClass(self.class)
		return
	}

    // 참조 할당하고, 오퍼랜드 스택에 쌓는다.
	ref := self.class.NewObj()
	frame.OperandStack().PushRef(ref)
}
```
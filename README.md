# UL
Unity Language used for all platforms,Can be converted to any other language

# 设计概述
整套系统包含4个部分：
1.用JSON文件格式表示的代码源文件。可以很方便的访问所有类，成员方法，成员函数，以及函数的实现代码。
2.代码转化器插件：每个插件都可以将一种类型的语言转化为JSON格式,并将JSON格式的源文件转化为指定的语言。理论上，JSON文件的源代码可以转化为任意语言，甚至自己实现的虚拟机语言。甚至直接编译成机器码可执行程序。
3.运行时接口，以及特定平台实现的库，或者是最终的虚拟机。只要实现这套运行时接口，则程序可以在目标平台运行。
4.标准库文件。基于运行时库构建的预定义的标准库，JSON文件表示。

# 设计目的
- 代码跨语言

  通过转化器插件，用一种语言写的代码，可以转化为任意语言，这能让各种分别擅长不同语言的程序员合作编程。
- 真跨平台

  首先，由于每个平台都有独自的语言，因此，得益于代码的跨平台，设计的程序可以编译成任意目标平台的代码。此跨平台不同于java，C#的跨平台之处在于，java,C#之类的语言，是编译成的最终可执行程序跨平台。而UL，则是程序的跨平台。
- 高开发效率

  UL语言首先解决了各自熟悉不同语言的程序员之间的鸿沟，减少程序员的学习成本。UL语言设计的指导原则是，工具能够帮程序员做的事情，决不让程序员去做。UL语言本身集多种语言的长处，例如：自动垃圾回收机制
- 高执行效率

  相对于java，C#等语言，由于UL代码可以编译成经过优化的目标平台程序，因此UL程序理论上执行效率更高。
- 高可优化性

  理论上，只需要实现目标平台的运行时库，代码就可以在目标平台运行，这提供了足够简单的模式。但是，如果对于性能有更加变态的需求，理论上，所有的标准库函数都可以特殊实现，甚至硬件实现。

# 用例

此工具多钟情况下可用，此处举例说明，但不包含所有用例：

- 1.跨语言跨平台合作开发
  UL语言的特性，使跨语言跨平台合作开发成为可能
- 2.用来当作脚本解释执行
  JSON格式表示的源码，方便的读取和访问。使编译器和虚拟机的实现变得非常简单。首先编译器不再需要了，只需要一个读取JSON文件，就可以获得所有元数据。以及函数实现代码的优化表示结构。只需要实现简单的几个接口，则可以实现一个虚拟机。

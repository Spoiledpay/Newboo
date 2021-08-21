import System.IO

f1 = StreamWriter("c:\\temp\\output.txt")
numbers as int = 26
f1.Write(numbers)
f1.Write(numbers)
f1.Close()
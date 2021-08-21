import System
import System.IO

print("Enter a filename:")
input = Console.ReadLine()
try:
    f1 = StreamReader(input)
except:
    print "There is no file named $input"
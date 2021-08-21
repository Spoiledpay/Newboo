import System
import System.IO

def inputNumber():
    Console.WriteLine("Pick a non-0 number")
try:
    inpX = int.Parse(Console.ReadLine())
    return inpX
except e as FormatException:
    Console.WriteLine("You must input a number")

inpNumber as int
while inpNumber == 0:
    inpNumber = inputNumber()
Console.WriteLine ("You picked the number: $inpNumber")
if inpNumber == 17:
    raise Exception("17 is your unlucky number")
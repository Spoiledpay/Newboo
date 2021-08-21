import System.IO
import System.Text

out = StringBuilder()
x as int
g = StreamReader("test.dat")
while g:
x = g.Read()
if x < 0:
break
else:
out.Append(System.Convert.ToChar(x))
g.Close()
print out
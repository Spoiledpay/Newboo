import System.IO

def filterFile(oldFile as string, newFile as
string):
    f1 = StreamReader(oldFile)
    f2 = StreamWriter(newFile)
for line in f1:
     if line == -1:
         break
if line[0] == char('#'):
    continue

f2.WriteLine(line)
f1.Close()
f2.Close()
import System.Runtime.InteropServices

[DllImport("msvcrt.dll", EntryPoint:"system")]

def System (sCommand as string):
	pass

System ("ECHO Hello world!")
print ""
System ("dir")
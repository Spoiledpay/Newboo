import System.Runtime.InteropServices

[DllImport("User32.dll", EntryPoint:"MessageBox")]
def msgbox(hwnd as int, msg as string, caption as string, msgtype as int):
	pass

def msgbox(msg as string):
	msgbox(0,msg,"Message",0)
	
msgbox(0, "MessageDialog called", "DllImport Demo", 0)

msgbox("one more time")
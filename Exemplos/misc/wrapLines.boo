#region license
// Copyright (c) 2003, 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion


import System.Text.RegularExpressions

def getWordBreaks(s as string):
	return m.Index for m as Match in /\b|$/.Matches(s)
	
def wrapLines(s as string, columns as int):
	
	lines = []
	
	nextBreak = columns	
	lastBreak = 0
	lineStart = 0
	for wb as int in getWordBreaks(s):
		
		if wb > nextBreak:
			line = s[lineStart:lastBreak]
			lines.Add(line.Trim())
			lineStart = lastBreak			
			nextBreak = lastBreak + columns
			
		lastBreak = wb
		
	lines.Add(s[lineStart:].Trim())
		
	return lines	
	
def test(s):
	print(join(wrapLines(s, 12), "*\n*"))
	
test("this is a very long string")
test("It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.")
		

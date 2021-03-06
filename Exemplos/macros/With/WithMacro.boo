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


import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast
import Boo.Lang.Compiler.Ast.Visitors
 
class WithMacro(AbstractAstMacro):
	
	private class NameExpander(DepthFirstTransformer):
		
		_inst as ReferenceExpression
		
		def constructor(inst as ReferenceExpression):
			_inst = inst
			
		override def OnReferenceExpression(node as ReferenceExpression):
			// if the name of the reference begins with '_'
			// then convert the reference to a member reference
			// of the provided instance
			if node.Name.StartsWith('_'):
				// create the new member reference and set it up
				mre = MemberReferenceExpression(node.LexicalInfo)
				mre.Name = node.Name[1:]
				mre.Target = _inst.CloneNode()
				
				// replace the original reference in the AST
				// with the new member-reference
				ReplaceCurrentNode(mre)
				
	override def Expand(macro as MacroStatement) as Statement:
		assert 1 == macro.Arguments.Count
		assert macro.Arguments[0] isa ReferenceExpression
		
		inst = macro.Arguments[0] as ReferenceExpression
		
		// convert all _<ref> to inst.<ref>
		block = macro.Body
		ne = NameExpander(inst)
		ne.Visit(block)
		return block

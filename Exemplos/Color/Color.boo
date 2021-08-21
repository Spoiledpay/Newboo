/*
CAMPOS
Black	0	A cor preta.
Blue	9	A cor azul.
Cyan	11	A cor ciano (verde-azul).
DarkBlue	1	A cor azul-escuro.
DarkCyan	3	A cor ciano-escuro (azul escuro-verde).
DarkGray	8	A cor cinza-escuro.
DarkGreen	2	A cor verde-escuro.
DarkMagenta	5	A cor magenta-escuro (arroxeado-escuro-vermelho).
DarkRed	4	A cor vermelho-escuro.
DarkYellow	6	A cor amarelo-escuro (ocre).
Gray	7	A cor cinza.
Green	10	A cor verde.
Magenta	13	A cor magenta (arroxeado-vermelho).
Red	12	A cor vermelha.
White	15	A cor branca.
Yellow	14	A cor amarela.
*/

import System

def Consolecolor(texto as string, backcolor as int, forecolor as int):
    Console.BackgroundColor = backcolor
    Console.ForegroundColor = forecolor
    Console.WriteLine(texto)
    #Console.ForegroundColor = ConsoleColor.White

def ColorReset():
    Console.BackgroundColor = ConsoleColor.Black
    Console.ForegroundColor = ConsoleColor.White


Consolecolor("Mundo", 0, 9)

ColorReset()




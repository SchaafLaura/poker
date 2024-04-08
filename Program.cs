using System;
// See https://projecteuler.net/problem=54
Console.WriteLine("Haiiii :3333");

class Hand
{
    Card[] cards;
}

class Card
{
    int value;
    Suite suite;
    
    public Card(string str)
    {

    }
}

enum Suite
{
    HEARTS,
    CLUBS,
    SPADES,
    DIAMONDS
}

/*
each card is represented by two chars:
V,S

V = card Value
S = card Suite
*/


/*
card Values:
2 = 2 
3 = 3
...
9 = 9
T = 10
J = 11
Q = 12
K = 13
A = 14
*/
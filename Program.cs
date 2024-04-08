using System;
using System.IO;
using System.Linq;
// See https://projecteuler.net/problem=54

string line;
StreamReader sr = new StreamReader("0054_poker.txt");
line = sr.ReadLine();

int player1Wins = 0;
while (line != null)
{
    // split line into 2char elements
    var split = line.Split(' ');

    // split into two parts - one for each player
    var split1 = split.Take(new Range(0, 5));
    var split2 = split.Take(new Range(5, 10));

    // create card objects from 2 chars
    var hand1 = new Card[5];
    var hand2 = new Card[5];
    int k = 0;
    foreach (var str in split1)
        hand1[k++] = new Card(str);
    k = 0;
    foreach (var str in split2)
        hand2[k++] = new Card(str);

    // create hands and compare them
    if (new Hand(hand1).CompareTo(new Hand(hand2)) > 0)
        player1Wins++;
    
    // read the next line from the file
    line = sr.ReadLine();
}
sr.Close();
Console.WriteLine(player1Wins);
Console.ReadLine();


class Hand : IComparable
{
    Card[] cards;

    public Hand(Card[] cards)
    {
        this.cards = cards;
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Hand)
            return -1;
        if (obj == this)
            return 0;


        throw new NotImplementedException();
    }
}

class Card : IComparable
{
    public int value;
    public Suite suite;

    public Card(string str)
    {

    }

    public int CompareTo(object? obj)
    {
        if (obj is not Card)
            throw new ArgumentException("Object wasn't a card :3");
        if (obj == this)
            return 0;
        return this.value.CompareTo(((Card)obj).value);
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

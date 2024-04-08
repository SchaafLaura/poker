using System;
using System.IO;
using System.Linq;
// See https://projecteuler.net/problem=54

string? line;
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
        Array.Sort(this.cards);
    }

    public Rank HeighestRank()
    {
        if (IsRoyalFlush())
            return Rank.ROYAL_FLUSH;

        // bla bla others here




        return Rank.HIGH_CARD;
    }

    /*TODO:
     * full house
     * two pair
     */




    private bool IsFourOfAKind()
    {
        return IsNOfAKind(4);
    }

    private bool IsThreeOfAKind()
    {
        return IsNOfAKind(3);
    }

    private bool IsOnePair()
    {
        return IsNOfAKind(2);
    }

    private bool IsNOfAKind(int n)
    {
        for (int i = n - 1; i < cards.Length; i++)
        {
            var val = cards[i].value;
            for (int j = 1; j < n; j++)
                if (val != cards[i - j].value)
                    return false;
        }
        return true;
    }

    private bool IsHighCard()
    {
        return true;
    }

    private bool IsRoyalFlush()
    {
        return IsStraightFlush() && cards[4].value == 14;
    }

    private bool IsStraightFlush()
    {
        return IsFlush() && IsStraight();
    }

    private bool IsFlush()
    {
        var suite = cards[0].suite;
        for (int i = 1; i < cards.Length; i++)
            if (suite != cards[i].suite)
                return false;
        return true;
    }

    private bool IsStraight()
    {
        for (int i = 1; i < cards.Length; i++)
            if (cards[i].value != cards[i - 1].value + 1)
                return false;
        return true;
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
        if (str.Count() != 2)
            throw new ArgumentException("`Card` class requires only 2 letters for the constructor");

        var ok = int.TryParse(str[0].ToString(), out value);

        if (!ok)
        {
            value = str[0] switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new Exception("AAAA")
            };
        }

        suite = str[1] switch
        {
            'H' => Suite.HEARTS,
            'C' => Suite.CLUBS,
            'S' => Suite.SPADES,
            'D' => Suite.DIAMONDS,
            _ => throw new Exception("EXPLODE")
        };
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

enum Rank
{
    HIGH_CARD       = 1000,
    ONE_PAIR        = 2000,
    TWO_PAIRS       = 3000,
    THREE_OF_A_KIND = 4000,
    STRAIGHT        = 5000,
    FLUSH           = 6000,
    FULL_HOUSE      = 7000,
    FOUR_OF_A_KIND  = 8000,
    STRAIGHT_FLUSH  = 9000,
    ROYAL_FLUSH     = 10000
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

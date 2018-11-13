using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTBot;


namespace QTBot.Modules.BlackJeck
{/*
    public class BlackJack : ModuleBase<SocketCommandContext>
    {
        public class Card
        {
            public enum CardRank
            {
                RANK_2,
                RANK_3,
                RANK_4,
                RANK_5,
                RANK_6,
                RANK_7,
                RANK_8,
                RANK_9,
                RANK_10,
                RANK_JACK,
                RANK_QUEEN,
                RANK_KING,
                RANK_ACE,
                MAX_RANK
            };

            public enum CardSuit
            {
                SUIT_CLUB,
                SUIT_DIAMOND,
                SUIT_HEART,
                SUIT_SPADE,
                MAX_SUIT
            };

            private CardRank m_rank;
            private CardSuit m_suit;

            public Card(CardRank rank = CardRank.RANK_2, CardSuit suit = CardSuit.SUIT_CLUB)
            {
                m_rank = rank;
                m_suit = suit;
            }

            public string GetCardString()
            {
                string card;
                switch (m_rank)
                {
                    case CardRank.RANK_2:
                        card = "2";
                        break;
                    case CardRank.RANK_3:
                        card = "3";
                        break;
                    case CardRank.RANK_4:
                        card = "4";
                        break;
                    case CardRank.RANK_5:
                        card = "5";
                        break;
                    case CardRank.RANK_6:
                        card = "6";
                        break;
                    case CardRank.RANK_7:
                        card = "7";
                        break;
                    case CardRank.RANK_8:
                        card = "8";
                        break;
                    case CardRank.RANK_9:
                        card = "9";
                        break;
                    case CardRank.RANK_10:
                        card = "10";
                        break;
                    case CardRank.RANK_JACK:
                        card = "J";
                        break;
                    case CardRank.RANK_QUEEN:
                        card = "Q";
                        break;
                    case CardRank.RANK_KING:
                        card = "K";
                        break;
                    case CardRank.RANK_ACE:
                        card = "A";
                        break;
                    default:
                        Console.WriteLine("m_rank is invalid.");
                        card = "?";
                        break;
                }
                switch (m_suit)
                {
                    case CardSuit.SUIT_CLUB:
                        card = card + "C";
                        break;
                    case CardSuit.SUIT_DIAMOND:
                        card = card + "D";
                        break;
                    case CardSuit.SUIT_HEART:
                        card = card + "H";
                        break;
                    case CardSuit.SUIT_SPADE:
                        card = card + "S";
                        break;
                    default:
                        Console.WriteLine("m_suit is invalid.");
                        card = card + "?";
                        break;
                }
                return card;
            }

            public int GetCardValue()
            {
                switch (m_rank)
                {
                    case CardRank.RANK_2: return 2;
                    case CardRank.RANK_3: return 3;
                    case CardRank.RANK_4: return 4;
                    case CardRank.RANK_5: return 5;
                    case CardRank.RANK_6: return 6;
                    case CardRank.RANK_7: return 7;
                    case CardRank.RANK_8: return 8;
                    case CardRank.RANK_9: return 9;
                    case CardRank.RANK_10: return 10;
                    case CardRank.RANK_JACK: return 10;
                    case CardRank.RANK_QUEEN: return 10;
                    case CardRank.RANK_KING: return 10;
                    case CardRank.RANK_ACE: return 11;
                    default: return 0;
                }
            }

            public void CheckForAce(int aceCount)
            {
                if (GetCardValue() == 11)
                {
                    ++aceCount;
                    //std::cout << "\nYou gained an ace.";
                }
                //std::cout << "\nYou have " << aceCount << " aces.";
            }
        };


        class Deck
        {
            List<Card> m_deck = new List<Card>();

            int m_cardIndex;

            int m_indexCount;



            static int GetRandomNumber(int max, int min)
            {
                Random rnd = new Random();
                int rand = rnd.Next(min, max);
                return rand;
            }

            static void SwapCards(Card x, Card y)
            {
                Card temp = x;
                x = y;
                y = temp;
            }

            public Deck()
            {
                m_cardIndex = 0;
                m_indexCount = 0;
                for (int suit = 0; suit < (int)Card.CardSuit.MAX_SUIT; ++suit)
                {
                    for (int rank = 0; rank < (int)Card.CardRank.MAX_RANK; ++rank)
                    {
                        m_deck.Add(new Card((Card.CardRank)rank, (Card.CardSuit)suit));
                    }
                }
                this.ShuffleDeck();
            }

            string GetDeckString()
            {
                string deckstring = "";
                for (int i = 0; i < 52; ++i)
                {
                    deckstring = deckstring + m_deck[i].GetCardString() + " ";
                }
                return deckstring;
            }

            public void ShuffleDeck()
            {
                for (int i = 0; i < 52; ++i)
                    SwapCards(m_deck[i], m_deck[Deck.GetRandomNumber(51, 0)]);
            }

            public Card DealCard()
            {
                if (m_indexCount >= 2)
                {
                    //std::cout << "\nCard index increased.";
                    m_indexCount = 0;
                    return m_deck[m_cardIndex++];
                }
                else
                {
                    ++m_indexCount;
                    //std::cout << "\nIndex count increased.";
                    return m_deck[m_cardIndex];
                }
            }
        };

        public enum GameResult
        {
            WIN,
            LOSS,
            TIE,
            ERROR
        };

        private class Choice
        {
            string m_choice = " ";
            SocketCommandContext m_context;

            public Choice(SocketCommandContext context)
            {
                m_context = context;
                GetPlayerChoice();
            }

            public string ReturnPlayerChoice()
            {
                return m_choice;
            }

            private async Task HandleSubChoiceAsync(SocketMessage arg)
            {
                var message = arg as SocketUserMessage;

                if (message is null || message.Author != m_context.User) return;

                m_choice = message.ToString();
            }

            private void GetPlayerChoice()
            {
                do
                {
                    // Get 'hit' or 'stand' from player
                    m_context.Client.MessageReceived += HandleSubChoiceAsync;
                } while (m_choice != "h" && m_choice != "s");
            }
        }

        void UseAce(int points, int aceCount)
        {
            while (points > 21 && aceCount > 0)
            {
                points -= 10;
                --aceCount;
                //std::cout << "\nAce used.";
            }
        }

        public async Task<GameResult> PlayBlackJack()
        {
            Console.WriteLine("Function start.");
            Deck deck = new Deck();
            Console.WriteLine("Deck initialised.");
            deck.ShuffleDeck();
            Console.WriteLine("Deck shuffled.");
            int playerSum = 0;
            int dealerSum = 0;
            int playerAce = 0;
            int dealerAce = 0;
            bool playerBlackJack = false;
            bool dealerBlackJack = false;
            
            //std::cout << "Better pony up for this game of chance.\n";
            Console.WriteLine("1");
            Console.WriteLine("Better pony up for this game of chance.");
            Console.WriteLine("2");
            // game start


            //std::cout << "\nThe dealer drew "; // dealer start
            Console.WriteLine($"The dealer drew {deck.DealCard().GetCardString()}");
            deck.DealCard().CheckForAce(dealerAce);
            dealerSum += deck.DealCard().GetCardValue(); // adds the value of the card to the sum
            UseAce(dealerSum, dealerAce);
            //std::cout << "\nThe dealer is on " << dealerSum << " points\n";
            Console.WriteLine($"The dealer is on {dealerSum}");


            while (true)  // player start
            {
                playerSum = 0;
                playerAce = 0;
                Console.WriteLine($"You drew {deck.DealCard().GetCardString()}");
                deck.DealCard().CheckForAce(playerAce);
                playerSum += deck.DealCard().GetCardValue();
                Console.WriteLine($"You drew {deck.DealCard().GetCardString()}");
                deck.DealCard().CheckForAce(playerAce);
                playerSum += deck.DealCard().GetCardValue();
                UseAce(playerSum, playerAce);
                if (playerSum == 21)
                {
                    playerBlackJack = true;
                    //std::cout << "\nYou got a blackjack!";
                    Console.WriteLine("You got a blackjack!");
                }
                //std::cout << "\nYou are on " << playerSum << " points\n";
                Console.WriteLine($"You are on {playerSum} points.");
                if (playerSum == 14) Console.WriteLine("Redrawing...");
                else break;
            }


            string answer;
            do  // player turn
            {
                //std::cout << "\nDo you hit or stand? ";
                Console.WriteLine("Do you hit or stand? (Enter 'h' or 's')");
                Choice choice = new Choice(Context);
                answer = choice.ReturnPlayerChoice();
                if (answer == "h")
                {
                    Console.WriteLine($"You drew {deck.DealCard().GetCardString()}");
                    deck.DealCard().CheckForAce(playerAce);
                    playerSum += deck.DealCard().GetCardValue();
                    UseAce(playerSum, playerAce);
                    Console.WriteLine($"You are on {playerSum} points");
                }
                if (answer == "s") Console.WriteLine($"You ended on {playerSum} points.");
                Console.WriteLine($"You ended on {playerSum} points");
            } while (playerSum < 21 && answer != "s");

            if (playerSum > 21) // check player for bust
            {
                Console.WriteLine($"You've gone and busted my good man.");
                return GameResult.LOSS;
            }

            for (int i = 0; dealerSum < 21; ++i) // dealer turn
            {
                if ((dealerSum < 17 && dealerSum <= playerSum))
                {
                    //std::cout << "\nThe dealer drew ";
                    deck.DealCard().GetCardString();
                    deck.DealCard().CheckForAce(dealerAce);
                    dealerSum += deck.DealCard().GetCardValue();
                    UseAce(dealerSum, dealerAce);
                    if (dealerSum == 21 && i == 0)
                    {
                        dealerBlackJack = true;
                        Console.WriteLine($"The dealer got a blackjack!");
                    }
                    Console.WriteLine($"The dealer is on {dealerSum} points");
                }
                else
                {
                    Console.WriteLine($"The dealer ended on {dealerSum} points");
                    break;
                }
            }


            if (dealerSum > 21) // check for dealer bust
            {
                Console.WriteLine($"The dealer busted.");
                return GameResult.WIN;
            }

            if (playerSum > dealerSum || (playerBlackJack && !dealerBlackJack))
                return GameResult.WIN;

            if (playerSum < dealerSum || (!playerBlackJack && dealerBlackJack))
                return GameResult.LOSS;

            if (playerSum == dealerSum)
                return GameResult.TIE;

            return GameResult.ERROR;
        }
    }*/
}

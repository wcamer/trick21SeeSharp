
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security.Cryptography;

// namespace declaration 
namespace Tricky21
{

    //Game class declaration
    class Game
    {

        // Main Method 
        static void Main(string[] args)
        {

            // statement 
            // printing Hello World! 
            Console.WriteLine("Welcome to Tricky 21 (written in C#)!!!");
            Console.WriteLine("\n What is your name");

            //string name = Console.ReadLine();
            string name = UserInput();
            Console.WriteLine($"Welcome to the game...{name}!  \n\nHere are the rules...");
            Console.WriteLine("Draw cards until the sum of your cards equals '21' most of the time.");
            Console.WriteLine("Each player will start out with 2 randomly drawn cards.");
            Console.WriteLine("Each player will choosing to 'hit' or stay.");
            Console.WriteLine("When a player hits, they draw a card and the other player gets turn to hit or stay");
            Console.WriteLine("When both plays stay, or when there are no more cards to draw, then the showdown happens.");
            Console.WriteLine("The cards are 1-11, there are no repeats and no royals or jokers...for now");
            Console.WriteLine("The loser of each round will lose 1 HP and the first to 0 HP loses\n\nRemember, this is a puzzle game...with luck too.  Have fun.\n\n");
           
            //Creates the table deck object
            Table_deck td = new Table_deck();
            //Creates the table deck
            td.Create_deck(td.deck);

            //player creation
            Player p = new Player();
            p.name = name;


            //cpu creation
            Player c = new Player();

            Console.WriteLine("Now the game starts\n\n");
        
            //Where the game starts
            Player_Turn(td, p, c, 1);





            // To prevents the screen from 
            // running and closing quickly 
            Console.ReadKey(); // end and exit of game
        }
        
       
        //Gets users input and returns a string
        static String UserInput()
        {
            Console.WriteLine($"Enter your response here");
            string said = Console.ReadLine();
            return said;

        }

        //Method where the game happens.
        static void Player_Turn(Table_deck table_Deck, Player player, Player cpu, int turn)

        {
           

            Player p = player;
            Player c = cpu;
            Table_deck td = table_Deck;
            int t = turn;

            //Starting cards for player and cpu
            if (p.hand.Count() == 0 && c.hand.Count() == 0)
            {
                Draw_card(td, p);
                Draw_card(td, p);
                Draw_card(td, c);
                Draw_card(td, c);
            }
            

            if ( t == 1 || t > 2)// if the turn counter gets higher than 2 then it will be reset to 1 which means the user is up
            {
                Console.WriteLine($"\n{player.name}'s turn....\n----------------------------------------------");
                
                turn = 1;
                
            }
           
            else
            {
       
                Console.WriteLine($"\n{cpu.name}'s turn....\n...........................................................");
                //Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            }

           

            if (td.deck.Count() > 0)
            {
                Console.WriteLine($"\nThere are {td.deck.Count()} card(s) left in the table deck\nYou may proceed..\n");
            }//end of at least 1 card in table deck check
            else
            {
                Console.WriteLine($"There are {td.deck.Count()} cards left in the table deck...\nBoth players are forced to stay and we move to the showdown!!!!\n\n");
                turn = 0;
                p.status = "stay";
                c.status = "stay";
                Showdown(p, c);
            }



            if (turn == 1)
            {
                Console.WriteLine($"{p.name}'s current sum/hand\nSum: {p.sum}");
                Console.WriteLine(string.Join(", ", p.hand));
                Console.WriteLine("\nDo you want to 'hit' or 'stay'?\nAcceptable Responses: 'h' , 'hit', 'Hit', or 'H' for hit.  Similar pattern for stay.\nHit = Draw a card, Stay = Keep the cards you have\n");
                string choice = UserInput();


                if (choice != "hit" && choice != "H" && choice != "Hit" && choice != "h" && choice != "stay" && choice != "S" && choice != "Stay" && choice != "s")
                {
                    Console.WriteLine("Invalid response, please try again");
                    Player_Turn(table_Deck, player, cpu, 1);
                }//end of input validation for hit and stay
                else
                {
                    
                    if (choice == "stay" || choice == "S" || choice == "Stay" || choice == "s")
                    {
                        p.status = "stay";
                        Console.WriteLine($"{p.name}, has chosen to...stay.");
                        //show report
                        report(p);

                        if (c.status == "hit") //check to see if cpu wants to keep drawing, if so then it goes to their turn
                        {
                            Player_Turn(td, p, c, 2);
                        }
                        else //if cpu is already at stay then it moves to showdown
                        {
                            Showdown(p, c);
                        }
                    }//end of player stay
                    else if (choice == "hit" || choice == "H" || choice == "Hit" || choice == "h")
                    {
                        if(p.sum < 21)//making sure the player doesn't intentionally/accidentially exceed 21 when they have 21 or higher
                        {
                            p.status = "hit";
                            Draw_card(td, p);
                            //show report
                            report(p);
                            c.status = "hit";
                            Player_Turn(td, p, c, 2);
                        }
                        else
                        {
                            Console.WriteLine($"\nSorry, '{p.name}' you have reached 21 or over.  You are forced to 'stay'\n");
                            
                            p.status = "stay";
                            //show report
                            report(p);
                            if (c.status == "hit") //check to see if cpu wants to keep drawing, if so then it goes to their turn
                            {
                                Player_Turn(td, p, c, 2);
                            }
                            else //if cpu is already at stay then it moves to showdown
                            {
                                Showdown(p, c);
                            }
                        }
                        

                    }// end of player hit



                }
            }//end of player turn

            else//begin of cpu turn
            {
                
                
             /*
                //List <string> cpu_hand_display = new List<string>(); 

                for (int i =0; i < c.hand.Count(); i++)
                {
                    c.cpu_hand_display.Add(c.hand[i].ToString());
                }
             */

                c.cpu_hand_display[0] = "?";//
                Console.WriteLine($"{c.name}'s Sum: ?/21..........\nHand:");//{c.sum}\nHand:");
                Console.WriteLine(string.Join(",",c.cpu_hand_display));

                if (c.sum <= 14)
                {
                    Console.WriteLine($"{c.name}, will draw a card\n");
                    Draw_card(td, c);
                    p.status = "hit";
                    c.status = "hit";
                    //show report
                    report(c);
                    Player_Turn(td, p, c, t + 1);
                }// end of always hit 14 or lower

                else if (c.sum == 15 || c.sum == 16)
                {
                    Console.WriteLine($"{c.name}, randomly decides.............");
                   
                    Random random = new Random();
                    int chance = random.Next(1, 11);
                    if (chance == 2 || chance == 3 || chance == 5 || chance == 8 || chance == 10)
                    {
                        Console.WriteLine("... it draw a card\n");
                        Draw_card(td, c);
                        
                        c.status = "hit";
                        p.status = "hit";
                        //show report
                        report(c);
                        Player_Turn(td, p, c, t + 1);

                    }
                    else
                    {
                        Console.WriteLine("... it will stay\n");
                        c.status = "stay";
                        //show report
                        report(c);

                        if (p.status == "hit")
                        {
                            Player_Turn(td, p, c, t + 1);
                        }
                        else
                        {
                            Showdown(p, c);
                        }
                        
                    }
               
                    
                     
                

                }// end of random draw or stay

                else //always will stay 17 or higher
                {
                    Console.WriteLine($"{c.name}, will stay...\n");
                    c.status = "stay";
                    report(c);
                    if (p.status == "hit")
                    {
                        Player_Turn(td, p, c, t + 1);
                    }
                    else
                    {
                        Showdown(p, c);
                    }

                }// end to always stay 17 or higher


                



            }
            Win_Check(td, p, c);
        }//end of turn method


        

        //This method will be ran when a player chooses to draw a card.
        static void Draw_card(Table_deck table_deck, Player turn_owner)// takes the table deck object and a player object
        {
            Random random = new Random();
            //randomly picks a number that will represent an index of the list of cards in the table deck
            int index_picker = random.Next(1, table_deck.deck.Count()); //top end is excluded but works because table deck is 0 based indexed

            //The card with the numeric value drawn from the table deck
            int card = table_deck.deck[index_picker];

            /*
            //Test section, Results: successful
            //Uncomment this section and comment out the "live" section to test
            Console.WriteLine($"Here is index picker...{index_picker} and here is the card...{card}\nHere is the deck now before the card '{card}' is taken out");
            Console.WriteLine(string.Join(", ", table_deck.deck));

            //removing card from table deck
            Console.WriteLine($"here is the table deck AFTER.......card '{card}' is taken out");
            table_deck.deck.RemoveAt(index_picker);
            Console.WriteLine(string.Join(", ",table_deck.deck));


            //before turn owner sum and hand are miniuplated 
            Console.WriteLine($"here is the turn owner...{turn_owner.name} and here is their sum before card '{card}' is added to sum....{turn_owner.sum}\n here is the turn owner hand below");
            Console.WriteLine(string.Join(", ", turn_owner.hand));

            //added the new card to their hand and sum
            turn_owner.sum += card;
            turn_owner.hand.Add(card);
            Console.WriteLine($"here is turn owner...{turn_owner.name}'s sum AFTER card is added ... {turn_owner.sum}\n below is their hand");
            Console.WriteLine(string.Join(", ", turn_owner.hand));
            */

            //live section
            if (turn_owner.name == "CPU") //urn_owner.hand.Count() == 0)
            {
                if(turn_owner.hand.Count() == 0)
                {
                    Console.WriteLine($"The player... '{turn_owner.name}' has drawn a MYSTERY CARD!!!");
                    table_deck.deck.RemoveAt(index_picker);
                    turn_owner.hand.Add(card);
                    turn_owner.sum += card;
                    turn_owner.cpu_hand_display.Add("?");
                }
                else
                {
                    Console.WriteLine($"The player... '{turn_owner.name}' has drawn the card...'{card}'");
                    table_deck.deck.RemoveAt(index_picker);
                    turn_owner.hand.Add(card);
                    turn_owner.sum += card;
                    turn_owner.cpu_hand_display.Add(card.ToString());
                }
                
            }
            else 
            {
                Console.WriteLine($"The player... '{turn_owner.name}' has drawn the card...'{card}'");
                table_deck.deck.RemoveAt(index_picker);
                turn_owner.hand.Add(card);
                turn_owner.sum += card;
            }
            







        }
        static void Showdown(Player player, Player cpu)
        {
            Console.WriteLine($"\nWe have now entered the showdown!!!!!!!\n");
            int target = 21;
            Player p = player;
            Player c = cpu;
                
            //check to see if anyone busted
            if (p.sum > 21 || c.sum > 21)
            {
                Console.WriteLine("Uh-oh...Someone busted");
                if(p.sum >21 && c.sum > 21) //both busted
                {
                    /*
                     //if both busted then find out who is closet to 21
                    int pdist = p.sum - target;
                    int cdist = c.sum - target;

                    if (pdist > p.dist)
                    {
                        Console.WriteLine($"'{p.name}' was the furthest over the target and loses 1 hp");
                        p.hp -= 1;
                        Console.WriteLine($"{p.name}'s hp.....{p.hp}");
                    }
                    */

                    //placeholder solution
                    Console.WriteLine("It looks like both players busted... both lose hp");
                    p.hp -= 1;
                    c.hp -= 1;
                    // show report


                }
                else if( p.sum >21 && c.sum <=21) // user busted but cpu didn't
                {
                    Console.WriteLine($"{p.name} busted at...{p.sum} and '{c.name}' didn't bust at...'{c.sum}'\n'{p.name}' loses 1 hp");
                    p.hp -= 1;

                    Report_both(p, c);

                }
                else // cpu busted but user didn't
                {
                    Console.WriteLine($"{c.name} busted at...{c.sum} and '{p.name}' didn't bust at...'{p.sum}'\n'{c.name}' loses 1 hp");
                    c.hp -= 1;

                    Report_both(p, c);
                }




            }//end of busted check
            else// if noone busted check who is closest to 21
            {
                Console.WriteLine("And the winner of this round is...\n");
                if (p.sum > c.sum)
                {
                    Console.WriteLine($"{p.name}!!!");
                    Console.WriteLine($"{p.name}: sum is...{p.sum}\n{c.name}: sum is {c.sum}");
                    c.hp -= 1;
                    Report_both(p, c);

                }
                else if (p.sum < c.sum)
                {
                    Console.WriteLine($"{c.name}!!!");
                    Console.WriteLine($"{p.name}: sum is...{p.sum}\n{c.name}: sum is {c.sum}");
                    p.hp -= 1;
                    Report_both(p, c);
                }
                else
                {
                    Console.WriteLine("It's a tie... Noone loses any points");
                    Console.WriteLine($"{p.name}: sum is...{p.sum}\n{c.name}: sum is {c.sum}");
                    Report_both(p, c);
                }

            }// end of if nobody busted

            //show report here?
            //testing win check here
            //Win_Check(p, c);

        }// end of showdown 

        static void Win_Check(Table_deck table_deck, Player player, Player Cpu)
        {
            Table_deck td = table_deck;
            Player p = player;
            Player c = Cpu;

            if (p.hp <= 0 && c.hp > 0) // Player has 0 hp (or less) and cpu has at least 1
            {
                Console.WriteLine("You lose...");
                Player_Check(p, c);
            }
            else if ( p.hp > 0 && c.hp <= 0) // Player has at least 1 hp and the cpu has 0 (or less)

            {
                Console.WriteLine($"Congratulations, '{p.name}' you win!!!!!!!!!");
                Player_Check(p, c);
            }
            else if (p.hp <= 0 && c.hp <= 0)// if player and cpu get 0 or less hp at the same time
            {
                Console.WriteLine($"It looks like both '{p.name}' and '{c.name}' are both have no more hp...\nThis calls for............\nSUDDEN DEATH!!!\n Each player will get 1 more hp to play another round!");
                p.hp = 1;
                c.hp = 1;
                Player_Check(p, c);
                Round_Reset(td, p, c);

                //next round
                Player_Turn(td, p, c, 1);
            }
            else // no winner has been determined
            {
                Console.WriteLine($"There's no winner yet so lets get to the next round!\n\n");

                Round_Reset(td, p, c);

                Console.WriteLine("Table deck is reset, player hand and cpu hands are dropped");


                Player_Check(p, c);
                Player_Turn(td, p, c, 1);

                




            }

        }

        /// <summary>
        /// //////////////////////////////////////////////////come back to this 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cpu"></param>
        /// 
        static void Player_Check(Player player, Player cpu)
        {
            Player p = player;
            Player c = cpu;
            Console.WriteLine($"\n++++++++++++++++++++++++++++++++++Player Check Report+++++++++++++++++++++++++++++++\n");
            Console.WriteLine($"Player: {p.name}  HP: {p.hp}\nPlayer: {c.name}  HP: {c.hp}\n");


        }
        static void Report_both(Player player, Player cpu)
        {
            Player p = player;
            Player c = cpu;


            Console.WriteLine($"**************************************Round End Report*************************************\n{p.name}\nSum: {p.sum}\nHand:");
            Console.WriteLine(string.Join(", ", p.hand));
            Console.WriteLine($"\n{c.name}\nSum:  {c.sum}/21 \nHand:");
            Console.WriteLine(string.Join(", ", c.hand));
            Console.WriteLine($"\n************************************End of Round End Report*****************************\n");



        }

        static void report(Player player)
        {
            Player p = player;

            if (p.name != "CPU" && p.name != "cpu" && p.name!= "Cpu")
            {
                Console.WriteLine($"\n============================={p.name} Report=============================\nSum: {p.sum}\nHand:");
                Console.WriteLine(string.Join(", ", p.hand));
                Console.WriteLine($"===========================End of report for {p.name}============================\n");
            }
            else
            {
                //to keep the first card for the cpu a mystery
               /*
                for (int i = 0; i < p.hand.Count(); i++)
                {
                    p.cpu_hand_display.Add(p.hand[i].ToString());
                }
                p.cpu_hand_display[0] = "?";
               */

                Console.WriteLine($"\n============================={p.name} Report=============================\nSum: ? + {p.sum - p.hand[0]}/21 \nHand:");
                Console.WriteLine(string.Join(", ", p.cpu_hand_display));
                Console.WriteLine($"===========================End of report for {p.name}============================\n");

            }
            
        }



        //this method will be ran after the showdown 
        static void Round_Reset( Table_deck table_deck, Player player, Player cpu )
        {
            //resets a given player's hand back to an empty hand
            static void HandDrop(Player player)
            {
                List<int> empty_hand = new List<int>();
                List<string> empty_cpu_display = new List<string>();
                player.hand = empty_hand;
                player.cpu_hand_display = empty_cpu_display;
                
            }
             
            /*
            //Testing Section, uncomment this section and comment out the "live" section to test out.
            //emptying the player's hand and minusing from sum
            Console.WriteLine("\nBefore returning.....\n");
            Console.WriteLine("Table deck");
            Console.WriteLine(string.Join(", ", table_deck.deck));
            Console.WriteLine("Player hand and sum");
            Console.WriteLine(string.Join(", ", player.hand));
            Console.WriteLine(player.sum);
            Console.WriteLine("Cpu hand and sum");
            Console.WriteLine(string.Join(", ", cpu.hand));
            Console.WriteLine(cpu.sum);



            // returns the cards from player and cpu to the table deck while decreasing their respective sums
            foreach(int card in player.hand)
            {
                table_deck.deck.Add(card);
                player.sum -= card;
                //player.hand.RemoveAt(card); // this doesn't work
                HandDrop(player);
               
            }

            foreach(int card in cpu.hand)
            {
                table_deck.deck.Add(card);
                cpu.sum -= card;
                HandDrop(cpu);

            }

          */
            /*
            Console.WriteLine("\nAfter returning.....\n");
            Console.WriteLine("Table deck");
            Console.WriteLine(string.Join(", ", table_deck.deck));
            Console.WriteLine("\nPlayer hand and sum");
            Console.WriteLine(string.Join(", ", player.hand));
            Console.WriteLine(player.sum);
            Console.WriteLine("\nCpu hand and sum");
            Console.WriteLine(string.Join(", ", cpu.hand));
            Console.WriteLine(cpu.sum);
            */
            


            //Live section

            // returns the cards from player and cpu to the table deck while decreasing their respective sums
            foreach (int card in player.hand)
            {
                table_deck.deck.Add(card);
                player.sum -= card;
                //player.hand.RemoveAt(card); // this doesn't work
                HandDrop(player);

            }

            foreach (int card in cpu.hand)
            {
                table_deck.deck.Add(card);
                cpu.sum -= card;
                HandDrop(cpu);

            }

            player.status = "hit";
            cpu.status = "hit";



        }//end of method



        


    }

   

    public class Player()
    {
        //Name of the player
        public string name = "CPU"; //It defaults to CPU but user will insert their own name

        //How many hit points the player has
        public int hp = 5; 

        //The  player's hand shown as a list
        public List<int> hand = new List<int>(); // this is where the cards will be held 

        //The sum of the player's card put together
        public int sum = 0; // this will show the sum of the cards of the player

        //Shows the status of the player.  Hit means the player chose to draw a card.  Stay means that player chose to not draw.  
        public string status = "hit"; //defaults to hit

        //This is the list that will display cpu cards plus the mysterious first card
        public List<string> cpu_hand_display = new List<string>();
        

    }

    public class Table_deck()
    {
        //initializes an empty list that will be manipulated
        public List<int> deck = new List<int>();

        //Adds card values 1-11 to the deck
        public void Create_deck(List<int> deck)
        {
           // List<int> deck = new List<int>();
            for (int i = 1; i < 12; i++)
                deck.Add(i);
            //Console.WriteLine(string.Join(", ", deck)); //displays the remaining cards in the table deck

        }

    }
    
    

}


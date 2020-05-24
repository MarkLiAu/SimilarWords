using System;
using System.Collections.Generic;
using System.Text;

namespace WordSimilarityLib
{
    /******************************************************************************************************
     * SuperMemo algorithm
     
        The author of SuperMemo states:

    Split the knowledge into smallest possible items.
    With all items associate an E-Factor equal to 2.5.
    Repeat items using the following intervals: I(1) :=1
    I(2) :=6
    for n>2: I(n) :=I(n-1)* EF
    where:
    I(n) - inter-repetition interval after the n-th repetition(in days),
    EF - E-Factor of a given item
    If interval is a fraction, round it up to the nearest integer.
    After each repetition assess the quality of repetition response in 0-5 grade scale: 
    5 - perfect response
    4 - correct response after a hesitation
    3 - correct response recalled with serious difficulty
    2 - incorrect response; where the correct one seemed easy to recall
    1 - incorrect response; the correct one remembered
    0 - complete blackout.
    After each repetition modify the E-Factor of the recently repeated item according to the formula:
    EF':=EF+(0.1-(5-q)*(0.08+(5-q)*0.02))
    where:
    EF' - new value of the E-Factor,
    EF - old value of the E-Factor,
    q - quality of the response in the 0-5 grade scale.
    If EF is less than 1.3 then let EF be 1.3.
    If the quality response was lower than 3 then start repetitions for the item from the beginning without changing the E-Factor (i.e.use intervals I(1), I(2) etc. as if the item was memorized anew).
    After each repetition session of a given day repeat again all items that scored below four in the quality assessment.Continue the repetitions until all of these items score at least four.

    Here are some terms that we will deal with when impementing the SuperMemo(SM-2) algorithm of spaced repetition.

    repetitions - this is the number of times a user sees a flashcard. 0 means they haven't studied it yet, 1 means it is their first time, and so on. It is also referred to as n in some of the documentation.
    quality - also known as quality of assessment.This is how difficult (as defined by the user) a flashcard is. The scale is from 0 to 5.
    easiness - this is also referred to as the easiness factor or EFactor or EF.It is multiplier used to increase the "space" in spaced repetition. The range is from 1.3 to 2.5.
    interval - this is the length of time (in days) between repetitions. It is the "space" of spaced repetition.
    nextPractice - This is the date/time of when the flashcard comes due to review again.
    ************************************************************************************************/
    public class FlashCard
    {
        public string name { get; set; }
        public int repetitions  { get; set; }
        public double easiness { get; set; }
        public int interval { get; set; }

        public FlashCard()
        {
            repetitions = 0;
            interval = 1;
            easiness = 2.5;
        }

        public FlashCard(string s):this()
        {
            name = s;
        }
    }

    public class SuperMemory2
    {
        public static FlashCard calculateSuperMemo2Algorithm(FlashCard card, int quality)
        {

            if (quality < 0 || quality > 5)
            {
                // throw error here or ensure elsewhere that quality is always within 0-5
            }

            // retrieve the stored values (default values if new cards)
            int repetitions = card.repetitions;
            double easiness = card.easiness;
            int interval = card.interval;

            // easiness factor
            easiness = Math.Max(1.3, easiness + 0.1 - (5.0 - quality) * (0.08 + (5.0 - quality) * 0.02));

            // repetitions
            if (quality < 3)
            {
                repetitions = 0;
            }
            else
            {
                repetitions += 1;
            }

            // interval
            if (repetitions <= 1)
            {
                interval = 1;
            }
            else if (repetitions == 2)
            {
                interval = 6;
            }
            else
            {
                interval = Convert.ToInt32( Math.Round(interval * easiness,0) );
            }

            // next practice 
            int millisecondsInDay = 60 * 60 * 24 * 1000;
            long now = DateTime.Now.Ticks;
            long nextPracticeDate = now + millisecondsInDay * interval;

            FlashCard result = new FlashCard();
            result.easiness = easiness;
            result.interval = interval;
            result.repetitions = repetitions;
            return result;

            // Store the nextPracticeDate in the database
            // ...
        }


        public static void test()
        {
            FlashCard card = new FlashCard();
            for(int n=0;n<30;n++)
            {
                card = calculateSuperMemo2Algorithm(card, 4);
                Console.WriteLine(n.ToString("####") + ": " + card.interval);
            }
                Console.ReadKey();
        }
    }
}

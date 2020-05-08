using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TestWebCore.api
{
    public class Word
    {
        public Word(string s)
        {
            name = s;
            pronounciation = "";
            honomony = "";
            explanationShort = "";
            explanationLong = "";
        }
        public string name { set; get; }
        public string pronounciation { get; set; }
        public int CollinsFrequency { get; set; }
        public int COCASequence { get; set; }
        public string honomony { get; set; }
        public string explanationShort { get; set; }
        public string explanationLong { get; set; }
    }
    public class Words
    {
        static public Dictionary<string, Word> WordList = new Dictionary<string, Word>();

        public Words()
        {
            if(WordList.Count<=0)
            {
                ReadCollins(5);
                ReadCollins(4);
                ReadCollins(3);
                ReadCollins(2);
                ReadCollins(1);
                ReadCOCA();
            }
        }

        static double WordCompare(string word1, string word2)
        {
            if (string.IsNullOrWhiteSpace(word1)) return 0;
            if (string.IsNullOrWhiteSpace(word2)) return 0;
            if (word1 == word2) return 1.0;
            if (word1.Contains(word2)) return word2.Length * 1.0 / word1.Length;
            if (word2.Contains(word1)) return word1.Length * 1.0 / word2.Length;

            int sameFront = 0;
            for (int i = 0; i < word1.Length && i < word2.Length; i++)
                if (word1[i] == word2[i]) sameFront++;
                else break;

            int sameBack = 0;
            for (int i = word1.Length - 1, j = word2.Length - 1; i >= sameFront && j >= sameFront; i--, j--)
                if (word1[i] == word2[j]) sameBack++;
                else break;

            return (sameFront + sameBack) * 1.0 / Math.Max(word1.Length, word2.Length);
        }

        public Word search(string name)
        {
            Word word = new Word(name);
            if (WordList.ContainsKey(name)) word = WordList[name];
            word.honomony = hononym(name);
            return word;
        }

        public string hononym(string name)
        {
            string result = "";
            SortedList<string, string> matchList = new SortedList<string, string>();

            foreach(var w in WordList)
            {
                //if (w.Key == name) continue;
                double val = WordCompare(name, w.Key);
                if (val < 0.7) continue;
                if (w.Value.COCASequence <= 0&&w.Value.CollinsFrequency>0) w.Value.COCASequence = (5 - w.Value.CollinsFrequency) * 2000;
                matchList.Add((1-val).ToString("0.000000")+w.Value.COCASequence.ToString("00000"), w.Key+"("+w.Value.CollinsFrequency+")("+w.Value.COCASequence+")");
            }
            if (matchList.Count <= 0) return "";
            foreach (var m in matchList) result+=","+ m.Value;
            return result.Substring(1);
        }

/*
Q: <b> abbey <br>ABBEY &nbsp;&nbsp;[<font face="Kingsoft Phonetic Plain">5Abi</font>]</b><br><br>
A: <font color=aqua><b>&#12304;Collins&#12305;</b></font>&nbsp;&nbsp;&#9670;&#9670;&#9671;&#9671;&#9671;<br><br><font color=green><b>abbey abbeys </b></font><br>An abbey is a church with buildings attached to it in which monks or nuns live or used to live.<br><font color=fuchsia><b>N-COUNT</b></font><br><br><font color=aqua><b>&#12304;&#31616;&#26126;&#35789;&#20856;&#12305;</b></font><br>n.<br>&#20462;&#36947;&#38498;, &#20462;&#36947;&#22763;&#65288;&#24635;&#31216;&#65289;<br><br><font color=aqua><b>&#12304;21&#19990;&#32426;&#33521;&#27721;&#35789;&#20856;&#12305;</b></font><br><font color=red>ab.bey</font><br><font color=orange><font face="Kingsoft Phonetic Plain">`AbI; 5Abi</font></font><br><< &#21517;&#35789;>><br><font color=green>1 a. (C) (&#30001; abbot &#25110; abbess &#25152;&#31649;&#29702;&#20043;) &#22823;&#20462;&#36947;&#38498;</font><br>b. [A~](C) (&#26366;&#20026;&#22823;&#20462;&#36947;&#38498;&#25110;&#23612;&#24245; &#20043;) &#22823;&#25945;&#22530; [&#23429;&#37048;] <br>c. [the A~]= Westminster Abbey<br><font color=green>2 (U) [the ~ ; &#38598;&#21512;&#31216;] &#20462;&#36947;&#38498;&#30340;&#20462;&#22763; [&#20462;&#22899;] </font><br><br>

Q: <b> ABC <br>ABC &nbsp;&nbsp;[<font face="Kingsoft Phonetic Plain">5eibi:5si:</font>]</b><br><br>
A: <font color=aqua><b>&#12304;Collins&#12305;</b></font>&nbsp;&nbsp;&#9670;&#9670;&#9671;&#9671;&#9671;<br><br><font color=green><b>1 ABC </b></font><br>The ABC of a subject or activity is the parts of it that you have to learn first because they are the most important and basic.<br><font color=blue>&nbsp;&nbsp;&nbsp;&nbsp;...the ABC of Marxism.</font><br><font color=fuchsia><b>N-SING: N of n</b></font><br><br><font color=green><b>2 ABC ABCs </b></font><br>Children who have learned their ABC or their ABCs have learned to recognize, write, or say the alphabet. (INFORMAL)<br><font color=fuchsia><b>N-COUNT: poss N</b></font><br><br><font color=aqua><b>&#12304;&#31616;&#26126;&#35789;&#20856;&#12305;</b></font><br>n.<br>&#23383;&#27597;&#34920;, &#22522;&#26412;&#30693;&#35782;<br>abbr.<br>&#32654;&#22269;&#24191;&#25773;&#20844;&#21496;<br>abbr.<br>&#28595;&#22823;&#21033;&#20122;&#24191;&#25773;&#20844;&#21496;<br><br><font color=aqua><b>&#12304;21&#19990;&#32426;&#33521;&#27721;&#35789;&#20856;&#12305;</b></font><br><font color=red>ABC</font><br>(&#30053;)American Broadcasting Company<br>&#32654;&#22269;&#24191;&#25773;&#20844;&#21496;<br>(&#19982;&#21733;&#20262;&#27604;&#20122;&#24191;&#25773;&#31995;&#32479; Columbia Broadcasting,&#31616;&#31216;CB S; &#22269;&#23478;&#24191;&#25773;&#20844;&#21496; National Broadcasting Company,&#31616;&#31216;NBC; &#24182;&#31216;&#20026;&#32654;&#22269;&#19977;&#22823;&#30005;&#35270; &#24191;&#25773;&#32593;)<br><br>

Q: <b> aboard <br>ABOARD &nbsp;&nbsp;[<font face="Kingsoft Phonetic Plain">E5bC:d</font>]</b><br><br>
A: <font color=aqua><b>&#12304;Collins&#12305;</b></font>&nbsp;&nbsp;&#9670;&#9670;&#9671;&#9671;&#9671;<br><br><font c
*/
        public void ReadCollins(int seq)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "CollinsL"+seq+"E.txt");
            string[] lines = File.ReadAllLines(file);
            string lastLine = "";
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("Q:"))
                {
                    lastLine = line;
                    continue;
                }

                if (!line.StartsWith("A:")) continue;
                if (!lastLine.StartsWith("Q:")) continue;

                int idx = lastLine.IndexOf("<br>");
                if (idx < 0) { lastLine = ""; continue; }   // wrong format, clear question line

                string name = lastLine.Substring(6, idx-6).Trim();
                Word word = new Word(name);
                word.CollinsFrequency = seq;
                word.explanationLong = line.Substring(3);

                WordList[name] = word;
            }
        }

/*
be	[bi:]	"<link href=""_collins_c.css"" rel=""stylesheet"" type=""text/css"" /><div class=""tab_content"" id=""dict_tab_101"" style=""display:block""><div class=""part_list""><ul><li><a href=""entry://#list_1"">1. AUXILIARY VERB USES 助动词用法</a></li><li><a href=""entry://#list_2"">2. OTHER VERB USES 其他动词用法</a></li></ul></div><div class=""part_main""><a name=""list_1""></a><div class=""break_bar""><div class=""part_list_d""><a href=""entry://#page_top""><span class=""part_number"">Part-1</span></a></div><b class=""break_bar_angle""></b></div><div class=""collins_content "" id =""menu_1""><div class=""explanation_item""><div class=""en_tip""><p>In spoken English, forms of <b>be</b> are often shortened, for example ‘I am’ is shortened to ‘I'm’ and ‘was not’ is shortened to ‘wasn't’.</p><p>在英语口语中，be经常使用缩合形式。如，I am 略作 I'm, was not 略作 wasn't。</p></div></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">1</a></span><span class=""explanation_label"" tid=""1_4764"">[AUX 助动词]</span><span class=""text_blue"">（和现在分词连用构成动词的进行式）</span> You use <b>be</b> with a present participle to form the continuous tenses of verbs. <b>be going to</b><span class=""text_gray"" style=""font-weight:bold；"">→see: </span><b class=""text_blue""><a class=""explain"" href=""entry://going"">going</a></b>； <span ><div class=""word_gram"" id=""word_gram_1_4764""><div ><div>&nbsp;&nbsp;[AUX -ing]</div><div>&nbsp;&nbsp;[AUX]</div></div></div></span></div><ul><li><p class=""sentence_en"">This is happening in every school throughout the country...</p><p>全国各地每所学校都在发生这样的事情。</p></li><li><p class=""sentence_en"">She didn't  always think carefully about what she was doing...</p><p>她对自己在做的事情并不总是考虑得很清楚。</p></li><li><p class=""sentence_en"">Pratt & Whitney has announced that it will <span class=""text_blue"">be</span> making further job reductions...</p><p>普惠公司宣布将进一步裁员。</p></li><li><p class=""sentence_en"">He had only <span class=""text_blue"">be</span>en trying to help...</p><p>他只是想尽力帮忙。</p></li><li><p class=""sentence_en"">He's doing <span class=""text_blue"">be</span>tter than I am.</p><p>他现在混得比我好。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">2</a></span><span class=""explanation_label"" tid=""2_4765"">[AUX 助动词]</span><span class=""text_blue"">（和过去分词连用构成被动语态）</span> You use <b>be</b> with a past participle to form the passive voice. <span ><div class=""word_gram"" id=""word_gram_2_4765""><div ><div>&nbsp;&nbsp;[AUX -ed]</div></div></div></span></div><ul><li><p class=""sentence_en"">Forensic experts were called in...</p><p>法医专家被请来。</p></li><li><p class=""sentence_en"">Her husband was killed in a car crash...</p><p>她的丈夫死于车祸。</p></li><li><p class=""sentence_en"">The cost of electricity from coal-fired stations is expected to fall...</p><p>用煤作燃料的火力发电站的成本有望降低。</p></li><li><p class=""sentence_en"">Similar action is <span class=""text_blue"">be</span>ing taken by the US government.</p><p>美国政府正在采取相似的行动。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">3</a></span><span class=""explanation_label"" tid=""3_4766"">[AUX 助动词]</span><span class=""text_blue"">（和不定式连用表示将来的安排或确定会发生的事情）</span> You use <b>be</b> with an infinitive to indicate that something is planned to happen, that it will definitely happen, or that it must happen. <b>be about to</b><span class=""text_gray"" style=""font-weight:bold；"">→see: </span><b class=""text_blue""><a class=""explain"" href=""entry://about"">about</a></b>； <span ><div class=""word_gram"" id=""word_gram_3_4766""><div ><div>&nbsp;&nbsp;[AUX to-inf]</div></div></div></span></div><ul><li><p class=""sentence_en"">The talks are to <span class=""text_blue"">be</span>gin tomorrow...</p><p>谈判将于明天开始。</p></li><li><p class=""sentence_en"">It was to <span class=""text_blue"">be</span> Johnson's first meeting with the board in nearly a month...</p><p>这将是近一个月来约翰逊首次和董事会碰面。</p></li><li><p class=""sentence_en"">You must take the whole project more seriously if you are to succeed...</p><p>如果你想成功的话，你必须更认真地对待整个项目。</p></li><li><p class=""sentence_en"">You are to answer to Brian, to take your orders from him.</p><p>你需要向布赖恩汇报，听从他的指挥。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">4</a></span><span class=""explanation_label"" tid=""4_4767"">[AUX 助动词]</span><span class=""text_blue"">（和不定式连用表示在某种情况下会发生什么事，应该怎样做或应该由谁来做）</span> You use <b>be</b> with an infinitive to say or ask what should happen or be done in a particular situation, how it should happen, or who should do it. <span ><div class=""word_gram"" id=""word_gram_4_4767""><div ><div>&nbsp;&nbsp;[AUX to-inf]</div></div></div></span></div><ul><li><p class=""sentence_en"">What am I to do without him?...</p><p>没有他，我该怎么办？</p></li><li><p class=""sentence_en"">Who is to say which of them had more power?...</p><p>谁来决定他们之中谁的权力应该更大一些？</p></li><li><p class=""sentence_en"">What is to <span class=""text_blue"">be</span> done?...</p><p>应该怎么做？</p></li><li><p class=""sentence_en"">Professor Hirsch is to <span class=""text_blue"">be</span> commended for bringing the state of our educational system to public notice.</p><p>在赫希教授的努力下，我们教育体系的现状引起了公众关注，为此对他应该给予嘉许。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">5</a></span><span class=""explanation_label"" tid=""5_4768"">[AUX 助动词]</span><span class=""text_blue"">（was和were和不定式连用，表示说话时间之后发生的事）</span> You use <b>was</b> and <b>were</b> with an infinitive to talk about something that happened later than the time you are discussing, and was not planned or certain at that time. <span ><div class=""word_gram"" id=""word_gram_5_4768""><div ><div>&nbsp;&nbsp;[AUX to-inf]</div></div></div></span></div><ul><li><p class=""sentence_en"">Then he received a phone call that was to change his life...</p><p>然后，他接到一个将改变他一生的电话。</p></li><li><p class=""sentence_en"">A few hours later he was to prove it.</p><p>几个小时之后他将证明这一点。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">6</a></span><span class=""explanation_label"" tid=""6_4769"">[AUX 助动词]</span><span class=""text_blue"">（表示可见到、可听到、可发现等）</span> You can say that something is <b>to be</b> seen, heard, or found in a particular place to mean that people can see it, hear it, or find it in that place. <span ><div class=""word_gram"" id=""word_gram_6_4769""><div ><div>&nbsp;&nbsp;[AUX -ed]</div></div></div></span></div><ul><li><p class=""sentence_en"">Little traffic was to <span class=""text_blue"">be</span> seen on the streets...</p><p>街上车辆很少。</p></li><li><p class=""sentence_en"">They are to <span class=""text_blue"">be</span> found all over the world.</p><p>它们遍布于世界各地。</p></li></ul></div></div></div><div class=""part_main""><a name=""list_2""></a><div class=""break_bar""><div class=""part_list_d""><a href=""entry://#page_top""><span class=""part_number"">Part-2</span></a></div><b class=""break_bar_angle""></b></div><div id =""menu_2""><div class=""explanation_item""><div class=""en_tip""><p>In spoken English, forms of <b>be</b> are often shortened, for example ‘I am’ is shortened to ‘I'm’ and ‘was not’ is shortened to ‘wasn't’.</p><p>在英语口语中，be经常使用缩合形式。如，I am 略作 I'm, was not 略作 wasn't。</p></div></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">1</a></span><span class=""explanation_label"" tid=""1_4770"">[V-LINK 连系动词]</span><span class=""text_blue"">（用于提供与主语相关的信息）</span> You use <b>be</b> to introduce more information about the subject, such as its identity, nature, qualities, or position. <span ><div class=""word_gram"" id=""word_gram_1_4770""><div ><div>&nbsp;&nbsp;[V n]</div><div>&nbsp;&nbsp;[V adj]</div><div>&nbsp;&nbsp;[V prep/adv]</div><div>&nbsp;&nbsp;[V]</div></div></div></span></div><ul><li><p class=""sentence_en"">She's my mother...</p><p>她是我母亲。</p></li><li><p class=""sentence_en"">This is Eliza<span class=""text_blue"">be</span>th Blunt, BBC, West Africa...</p><p>英国广播公司的伊丽莎白·布伦特在西非为您报道。</p></li><li><p class=""sentence_en"">He is a very attractive man...</p><p>他是一个很有魅力的男人。</p></li><li><p class=""sentence_en"">My grandfather was a butcher...</p><p>我祖父是个屠夫。</p></li><li><p class=""sentence_en"">The fact that you were willing to pay in the end is all that matters...</p><p>最后你愿意付钱才是最重要的。</p></li><li><p class=""sentence_en"">He is fifty and has <span class=""text_blue"">be</span>en through two marriages...</p><p>他今年50岁，经历过两次婚姻。</p></li><li><p class=""sentence_en"">The sky was black...</p><p>天空一片漆黑。</p></li><li><p class=""sentence_en"">It is 1,267 feet high...</p><p>它有1,267英尺高。</p></li><li><p class=""sentence_en"">Cheney was in Madrid...</p><p>切尼当时在马德里。</p></li><li><p class=""sentence_en"">His house is next door...</p><p>他的房子就在隔壁。</p></li><li><p class=""sentence_en"">Their last major film project was in 1964...</p><p>他们上一个重要电影项目完成于1964年。</p></li><li><p class=""sentence_en"">'Is it safe?' — 'Well of course it is.'...</p><p>“安全吗？”——“当然啦。”</p></li><li><p class=""sentence_en"">He's still alive isn't  he?</p><p>他还活着，不是吗？</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">2</a></span><span class=""explanation_label"" tid=""2_4771"">[V-LINK 连系动词]</span><span class=""text_blue"">（以it作主语，用来进行描述或作出判断）</span> You use <b>be</b>, with 'it' as the subject, in clauses where you are describing something or giving your judgment of a situation. <span ><div class=""word_gram"" id=""word_gram_2_4771""><div ><div>&nbsp;&nbsp;[<l>it</l> V adj]</div><div>&nbsp;&nbsp;[<l>it</l> V adj to-inf]</div><div>&nbsp;&nbsp;[<l>it</l> V adj that]</div><div>&nbsp;&nbsp;[<l>it</l> V adj -ing]</div><div>&nbsp;&nbsp;[<l>it</l> V n that]</div><div>&nbsp;&nbsp;[<l>it</l> V n -ing]</div><div>&nbsp;&nbsp;[<l>it</l> V n to-inf]</div><div>&nbsp;&nbsp;[<l>it</l> V prep to-inf]</div></div></div></span></div><ul><li><p class=""sentence_en"">It was too chilly for swimming...</p><p>这时候游泳太冷了。</p></li><li><p class=""sentence_en"">Sometimes it is necessary to say no...</p><p>有时候拒绝是必要的。</p></li><li><p class=""sentence_en"">It is likely that investors will face losses...</p><p>投资者们可能要面临损失。</p></li><li><p class=""sentence_en"">It's nice having friends to chat to...</p><p>有朋友聊聊天是很惬意的。</p></li><li><p class=""sentence_en"">It's a good thing I brought lots of handkerchiefs...</p><p>还好我买了很多手帕。</p></li><li><p class=""sentence_en"">It's no good just having meetings...</p><p>光开会是没有用的。</p></li><li><p class=""sentence_en"">It's a good idea to avoid refined food...</p><p>最好少吃精加工食品。</p></li><li><p class=""sentence_en"">It's up to us to prove it.</p><p>这得靠我们来证明。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">3</a></span><span class=""explanation_label"" tid=""3_4772"">[V-LINK 连系动词]</span><span class=""text_blue"">（与非人称代词there连用构成there is和there are表示存在或发生）</span> You use <b>be</b> with the impersonal pronoun 'there' in expressions like <b>there is</b> and <b>there are</b> to say that something exists or happens. <span ><div class=""word_gram"" id=""word_gram_3_4772""><div ><div>&nbsp;&nbsp;[<l>there</l> V n]</div></div></div></span></div><ul><li><p class=""sentence_en"">Clearly there is a problem here...</p><p>显然，这里出了个问题。</p></li><li><p class=""sentence_en"">There are very few cars on this street...</p><p>这条街道上车辆很少。</p></li><li><p class=""sentence_en"">There was nothing new in the letter...</p><p>信里没有什么新的内容。</p></li><li><p class=""sentence_en"">There were always things to think about when she went walking.</p><p>她去散步的时候总是有一些事情要考虑。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">4</a></span><span class=""explanation_label"" tid=""4_4773"">[V-LINK 连系动词]</span><span class=""text_blue"">（表示主语和从句和其他从句结构之间的某种联系）</span> You use <b>be</b> as a link between a subject and a clause and in certain other clause structures, as shown below. <span ><div class=""word_gram"" id=""word_gram_4_4773""><div ><div>&nbsp;&nbsp;[V n]</div><div>&nbsp;&nbsp;[V to-inf]</div><div>&nbsp;&nbsp;[V -ing]</div><div>&nbsp;&nbsp;[V wh]</div><div>&nbsp;&nbsp;[V that]</div><div>&nbsp;&nbsp;[V as if]</div></div></div></span></div><ul><li><p class=""sentence_en"">It was me she didn't like, not what I represented...</p><p>她不喜欢的是我，而不是我的陈述。</p></li><li><p class=""sentence_en"">What the media should not do is to exploit people's natural fears...</p><p>媒体不应该利用人们天生的恐惧心理。</p></li><li><p class=""sentence_en"">Our greatest problem is convincing them...</p><p>我们最大的问题就是要说服他们。</p></li><li><p class=""sentence_en"">The question was whether protection could <span class=""text_blue"">be</span> improved...</p><p>问题在于是否能够加强保护。</p></li><li><p class=""sentence_en"">All she knew was that I'd had a broken marriage...</p><p>她只知道我的婚姻已经破裂。</p></li><li><p class=""sentence_en"">Local residents said it was as if there had <span class=""text_blue"">be</span>en a nuclear explosion.</p><p>当地的居民说就好像发生了核爆炸一样。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">5</a></span><span class=""explanation_label"" tid=""5_4774"">[V-LINK 连系动词]</span><span class=""text_blue"">（用在如the thing is和the point is这样的结构中，引导表示陈述或提出观点的从句）</span> You use <b>be</b> in expressions like <b>the thing is</b> and <b>the point is</b> to introduce a clause in which you make a statement or give your opinion. <span ><div class=""word_gram"" id=""word_gram_5_4774""><div ><div>&nbsp;&nbsp;[V cl]</div><div>&nbsp;&nbsp;[SPOKEN 口语]</div></div></div></span></div><ul><li><p class=""sentence_en"">The fact is, the players gave everything they had...</p><p>事实上，选手们尽了全力。</p></li><li><p class=""sentence_en"">The plan is good； the problem is it doesn't  go far enough.</p><p>计划不错；问题在于不够深入。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">6</a></span><span class=""explanation_label"" tid=""6_4775"">[V-LINK 连系动词]</span><span class=""text_blue"">（用在如to be fair, to be honest或to be serious 这样的结构中表示尽量）</span> You use <b>be</b> in expressions like <b>to be fair</b> ,<b>to be honest</b>, or <b>to be serious</b> to introduce an additional statement or opinion, and to indicate that you are trying to be fair, honest, or serious. <span ><div class=""word_gram"" id=""word_gram_6_4775""><div ><div>&nbsp;&nbsp;[V adj]</div></div></div></span></div><ul><li><p class=""sentence_en"">She's always noticed. But then, to <span class=""text_blue"">be</span> honest, Ghislaine likes <span class=""text_blue"">be</span>ing noticed...</p><p>她总是受到关注。但是说句实在话，吉莱纳喜欢被人关注。</p></li><li><p class=""sentence_en"">It enabled students to devote more time to their studies, or to <span class=""text_blue"">be</span> more accurate, more time to relaxation.</p><p>它可以让学生们有更多的时间来学习，或者更准确一点说，有更多的时间来放松自己。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">7</a></span><span class=""explanation_label"" tid=""7_4776"">[V-LINK 连系动词]</span><span class=""text_blue"">（有时用来代替现在时态中be的几个常规形式，尤用于whether后）</span> The form '<b>be</b>' is used occasionally instead of the normal forms of the present tense, especially after 'whether'. <span ><div class=""word_gram"" id=""word_gram_7_4776""><div ><div>&nbsp;&nbsp;[<l>be</l> n]</div><div>&nbsp;&nbsp;[FORMAL 正式]</div></div></div></span></div><ul><li><p class=""sentence_en"">The chemical agent, whether it <span class=""text_blue"">be</span> mustard gas or nerve gas, can <span class=""text_blue"">be</span> absor<span class=""text_blue"">be</span>d by the skin.</p><p>这类化学制剂，不管是芥子气还是神经瓦斯，都会被皮肤吸收。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">8</a></span><span class=""explanation_label"" tid=""8_4777"">[VERB 动词]</span><span class=""text_blue"">存在</span> If something <b>is</b>, it exists. <span ><div class=""word_gram"" id=""word_gram_8_4777""><div ><div>&nbsp;&nbsp;[V]</div><div>&nbsp;&nbsp;[mainly FORMAL or LITERARY 主正式或文]</div></div></div></span></div><ul><li><p class=""sentence_en"">It hurt so badly he wished to cease to <span class=""text_blue"">be</span>.</p><p>他觉得疼痛难忍，恨不得死了算了。</p></li><li><p class=""sentence_en"">...to <span class=""text_blue"">be</span> or not to <span class=""text_blue"">be</span>.</p><p>活着还是死去</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">9</a></span><span class=""explanation_label"" tid=""9_4778"">[V-LINK 连系动词]</span><span class=""text_blue"">保持真我；按自己的方式行事；显常态</span> To <b>be yourself</b> means to behave in the way that is right and natural for you and your personality. <span ><div class=""word_gram"" id=""word_gram_9_4778""><div ><div>&nbsp;&nbsp;[V pron-refl]</div></div></div></span></div><ul><li><p class=""sentence_en"">She'd learnt to <span class=""text_blue"">be</span> herself and to stand up for her convictions.</p><p>她已经学会了按自己的方式行事，坚持自己的信仰。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">10</a></span><span class=""explanation_label"" tid=""10_4779"">[PHRASE 短语]</span><span class=""text_blue"">非常；极为</span> If someone or something is, for example, <b>as</b> happy <b>as can be</b> or <b>as</b> quiet <b>as could be</b>, they are extremely happy or extremely quiet. <span ><div class=""word_gram"" id=""word_gram_10_4779""><div ><div>&nbsp;&nbsp;[usu v-link PHR]</div></div></div></span></div><ul></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">11</a></span><span class=""explanation_label"" tid=""11_4780"">[PHRASE 短语]</span><span class=""text_blue"">如果不是…的话；如果没有…的话</span> If you talk about what would happen <b>if it wasn't for</b> someone or something, you mean that they are the only thing that is preventing it from happening. <span ><div class=""word_gram"" id=""word_gram_11_4780""><div ><div>&nbsp;&nbsp;[V inflects]</div></div></div></span></div><ul><li><p class=""sentence_en"">I could happily move back into a flat if it wasn't  for the fact that I'd miss my garden...</p><p>如果不是因为我会想念自己的花园的话，我会很乐意搬回公寓住。</p></li><li><p class=""sentence_en"">If it hadn't <span class=""text_blue"">be</span>en for her your father would <span class=""text_blue"">be</span> alive today.</p><p>如果不是因为她，你父亲今天可能还活着。</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">12</a></span><span class=""explanation_label"" tid=""12_4781"">[PHRASE 短语]</span><span class=""text_blue"">尽管那样；即便如此</span> You say '<b>Be that as it may</b>' when you want to move onto another subject or go further with the discussion, without deciding whether what has just been said is right or wrong. <span ><div class=""word_gram"" id=""word_gram_12_4781""><div ><div>&nbsp;&nbsp;[vagueness]</div></div></div></span></div><ul><li><p class=""sentence_en"">'Is he still just as fat?' — 'I wouldn't know,' continued her mother, ignoring the interruption, 'and <span class=""text_blue"">be</span> that as it may, he has made a fortune.'</p><p>“他还是那么胖吗？”——“我不知道，”她妈妈接着说，没有理睬这一打岔，“就算那样，他已经发财了。”</p></li></ul></div><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">13</a></span><span class=""explanation_label"" tid=""13_4782"">[PHRASE 短语]</span><span class=""text_blue"">身体不舒服；身体不适</span> If you say that you <b>are not yourself</b>, you mean you are not feeling well. <span ><div class=""word_gram"" id=""word_gram_13_4782""><div ><div>&nbsp;&nbsp;[V inflects]</div></div></div></span></div><ul><li><p class=""sentence_en"">She is not herself. She came near to a breakdown.</p><p>她身体不舒服，简直要崩溃了。</p></li></ul></div></div></div></div>"	COCA20000 : 2 | Collins : ★★★★★	"To <b><u>be</u></b> is to exist, or to take place. As Hamlet best put it: ""To <b><u>be</u></b> or not to <b><u>be</u></b>, that is the question.""<hr>The verb <b><u>be</u></b> is one of the most frequently used words in English, and it often takes the form of <b><u>am</u></b>, <b><u>are</u></b>, <b><u>were</u></b>, or <b><u>was</u></b>. When you make plans to meet someone later, you could say, ""I'll <b><u>be</u></b> on the steps in front of the library,"" and when you talk about your goals, you might confess, ""I want to <b><u>be</u></b> a movie star."" It's a verb with a complicated history, stemming mainly from the Old English <b><u>bēon</u></b>, ""be, exist, or happen."""			
and	[ənd]	"<link href=""_collins_c.css"" rel=""stylesheet"" type=""text/css"" /><div class=""tab_content"" id=""dict_tab_101"" style=""display:block""><div class=""part_main""><div class=""collins_content""><div class=""explanation_item""><div class=""explanation_box""><span class=""item_number""><a href=""entry://#page_top"">1</a></span><span class=""explanation_label"" tid=""1_2025"">[CONJ-COORD 连词]</span><span class=""text_blue"">（连接两个以上的单词、词组或子句）和，与，同</span> You use <b>and</b> to link two or more words, groups, 
*/
        public void ReadCOCA500()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "COCA20000.txt");
            string[] lines = File.ReadAllLines(file);
            int seq = 0;
            foreach (string line in lines)
            {
                string [] ss = line.Split("\t");
                if (ss.Length < 3) continue;

                int idx = line.IndexOf("[");
                int idx2 = line.IndexOf("]");
                if (idx < 0 || idx2 <= idx) continue;
                string name = line.Substring(0, idx).Trim();

                name = ss[0];
                Word word = new Word(name);
                if (WordList.ContainsKey(name)) word = WordList[name];
                word.pronounciation = ss[1];         //  line.Substring(idx, idx2 - idx + 1);
                word.COCASequence = ++seq;
                if (string.IsNullOrWhiteSpace(word.explanationLong)) word.explanationLong = ss[2];      // line.Substring(idx2);

                WordList[name] = word;
            }
        }

        // list only: frequency word
        public void ReadCOCA()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "COCAList20000.txt");
            string[] lines = File.ReadAllLines(file);
            int seq = 0;
            foreach (string line in lines)
            {
                string[] ss = line.Split(" ");
                if (ss.Length < 2) continue;

                string name = ss[1];
                Word word = new Word(name);
                if (WordList.ContainsKey(name)) word = WordList[name];
                // word.pronounciation = ss[1];         //  line.Substring(idx, idx2 - idx + 1);
                word.COCASequence = ++seq;
                WordList[name] = word;
            }
        }

    }

    public class Tools
    {
        public static string AppCoreDataFolder()
        {
            var userPath = Environment.GetEnvironmentVariable(
              RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
              "LOCALAPPDATA" : "Home");

            var assy = System.Reflection.Assembly.GetEntryAssembly();
            var companyName = assy.GetCustomAttributes<AssemblyCompanyAttribute>()
              .FirstOrDefault();
            var path = System.IO.Path.Combine(userPath, companyName.Company);

            return path;
        }

    }
}

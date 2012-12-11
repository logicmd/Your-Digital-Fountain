NativeTest
==========
Testbed for my experiment.

### Decoding Probability ###

Decoding Probability does not grow linearly along with overhead. It is only affected by the abusolute value of overhead.

To increase the probability to decode, it is necessary to collect more than k drops, The number of excess drops is the overhead; the bigger the overhead, the higher the decoding probability.

![decoding probability](https://raw.github.com/logicmd/Your-Digital-Fountain/master/NaiveTest/graph.jpg)

Note that the overhead is independent of k: 20 excess drops to get 10-6 failure probability for any k (then better if K is large).


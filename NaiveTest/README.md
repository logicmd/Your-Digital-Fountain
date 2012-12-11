NativeTest
==========
Testbed for my experiment.

### Decoding Probability ###

Decoding Probability does not grow linearly along with overhead.
Decoding Probability is only affected by the abusolute value of overhead.

To increase the probability to decode, it is necessary to collect more than k drops, The number of excess drops is the overhead; the bigger the overhead, the higher the decoding probability.

![decoding probability](/graph.jpg)

Note that the overhead is independent of k: 20 excess drops to get 10-6 failure probability for any k (then better if K is large).


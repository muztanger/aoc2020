using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day25
    {
        private static long Decode(IEnumerable<string> input)
        {
            long cardPublicKey = long.Parse(input.First());
            long doorPublicKey = long.Parse(input.Skip(1).First());

            // door has other secret loop size

            // handshake

            //The card transforms the subject number of 7 according to the card's secret loop size. 
            //The result is called the card's public key.
            //long cardSecretLoopSize = 1;
            //long cardPublicKey = Transform(7, cardSecretLoopSize);

            //The door transforms the subject number of 7 according to the door's secret loop size. 
            //The result is called the door's public key.
            //long doorSecretLoopSize = 1;
            //long doorPublicKey = Transform(7, doorSecretLoopSize);

            //The card and door use the wireless RFID signal to transmit the two public keys
            //(your puzzle input) to the other device.Now, the card has the door's public key, 
            // and the door has the card's public key.Because you can eavesdrop on the signal, 
            //you have both public keys, but neither device's loop size.
            long cardSecretLoopSize = InvTransform(7, cardPublicKey);
            long doorSecretLoopSize = InvTransform(7, doorPublicKey);

            //The card transforms the subject number of the door's public key according to the card's loop size.
            //The result is the encryption key.
            long encryptionKey = Transform(doorPublicKey, cardSecretLoopSize);

            //The door transforms the subject number of the card's public key according to the door's loop size.
            //The result is the same encryption key as the card calculated.
            long encryptionKey2 = Transform(cardPublicKey, doorSecretLoopSize);
            Assert.AreEqual(encryptionKey, encryptionKey2);
            return encryptionKey;
        }

        private static long InvTransform(long subjectNumber, long publicKey)
        {
            long value = 1;
            long loopSize = 0;
            while (value != publicKey)
            {
                value = (value * subjectNumber) % 20201227;
                loopSize++;
            }
            return loopSize;
        }

        private static long Transform(long subjectNumber, long loopSize)
        {
            long value = 1;
            while (loopSize > 0)
            {
                value = (value * subjectNumber) % 20201227;
                loopSize--;
            }
            return value;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"5764801
17807724";
            var parsed = Decode(Common.GetLines(input));
            Assert.AreEqual(14897079, parsed);
        }

        [Test]
        public void Part1()
        {
            var parsed = Decode(Common.DayInput(nameof(Day25)));
            Assert.AreEqual(9177528, parsed);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            var parsed = Decode(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            var parsed = Decode(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            var parsed = Decode(Common.DayInput(nameof(Day25)));
            Assert.AreEqual(0, 1);
        }

    }
}

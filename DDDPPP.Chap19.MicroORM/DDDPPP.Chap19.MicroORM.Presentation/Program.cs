﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.MicroORM.Application;
using DDDPPP.Chap19.MicroORM.Application.Application.BusinessTasks;
using DDDPPP.Chap19.MicroORM.Application.Application.Queries;
using StructureMap;

namespace DDDPPP.Chap19.MicroORM.Presentation
{
    public class Program
    {
        private static Dictionary<Guid, String> members = new Dictionary<Guid, string>();

        public static void Main(string[] args)
        {
            Bootstrapper.Startup();

            var memberIdA = Guid.NewGuid();
            var memberIdB = Guid.NewGuid();

            members.Add(memberIdA, "Ted");
            members.Add(memberIdB, "Rob");

            var auctionId = CreateAution();
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);

            // Bid
            Bid(auctionId, memberIdA, 10m);
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();

            // Bid
            Bid(auctionId, memberIdB, 1.49m);
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();

            // Bid
            Bid(auctionId, memberIdB, 10.01m);
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();

            // Bid
            Bid(auctionId, memberIdB, 12.00m);
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();

            // Bid
            Bid(auctionId, memberIdA, 12.00m);
            PrintStatusOfAuctionBy(auctionId);
            PrintBidHistoryOf(auctionId);
            Console.WriteLine("Hit any key to continue");
            Console.ReadLine();

            Console.ReadLine();
        }

        public static Guid CreateAution()
        {
            var createAuction = ObjectFactory.GetInstance<CreateAuction>();

            var auctionCreation = new AuctionCreation();

            auctionCreation.StartingPrice = 0.99m;
            auctionCreation.EndsAt = DateTime.Now.AddDays(1);

            var auctionId = createAuction.Create(auctionCreation);

            return auctionId;
        }

        public static void Bid(Guid auctionId, Guid memberId, decimal amount)
        {
            var bidOnAuction = ObjectFactory.GetInstance<BidOnAuction>();

            bidOnAuction.Bid(auctionId, memberId, amount);
        }

        public static void PrintStatusOfAuctionBy(Guid auctionId)
        {
            var auctionSummaryQuery = ObjectFactory.GetInstance<AuctionSummaryQuery>();
            var status = auctionSummaryQuery.AuctionStatus(auctionId);

            Console.WriteLine("No Of Bids: " + status.NumberOfBids);
            Console.WriteLine("Current Bid: " + status.CurrentPrice.ToString("##.##"));
            Console.WriteLine("Winning Bidder: " + FindNameOfBidderWith(status.WinningBidderId));
            Console.WriteLine("Time Remaining: " + status.TimeRemaining());
            Console.WriteLine();
        }

        public static void PrintBidHistoryOf(Guid auctionId)
        {
            var bidHistoryQuery = ObjectFactory.GetInstance<BidHistoryQuery>();
            var status = bidHistoryQuery.BidHistoryFor(auctionId);

            Console.WriteLine("Bids..");

            foreach (var bid in status)
                Console.WriteLine(FindNameOfBidderWith(bid.Bidder) + "\t - " + bid.AmountBid.ToString("G") + "\t at " + bid.TimeOfBid);
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }

        public static string FindNameOfBidderWith(Guid id)
        {
            if (members.ContainsKey(id))
                return members[id];
            else
                return string.Empty;
        }
    }
}
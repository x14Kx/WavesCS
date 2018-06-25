﻿using System;
using System.Collections.Generic;
using System.IO;

namespace WavesCS
{
    public class SponsoredFeeTransaction : Transaction
    {
        public decimal Fee { get; }
        public decimal MinimalFeeInAssets { get; }
        public Asset Asset { get; }

        public SponsoredFeeTransaction(byte[] senderPublicKey, Asset asset, decimal minimalFeeInAssets, decimal fee = 100000000) :
            base(senderPublicKey)
        {
            Asset = asset;            
            Fee = fee;
            MinimalFeeInAssets = minimalFeeInAssets;
        }

        public readonly byte Version = 1;

        public override byte[] GetBody()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {                   
                writer.Write(TransactionType.SponsoredFee);                
                writer.Write(Version);
                writer.Write(SenderPublicKey);
                writer.Write(Asset.Id.FromBase58());
                writer.WriteLong(Asset.AmountToLong(MinimalFeeInAssets));
                writer.WriteLong(Timestamp.ToLong());
                writer.WriteLong(Assets.WAVES.AmountToLong(Fee));                                                         

                return stream.ToArray();
            }                
        }

        protected override bool SupportsProofs()
        {
            return true;
        }

        public override Dictionary<string, object> GetJson() => new Dictionary<string, object>
            {
                {"type", TransactionType.SponsoredFee},
                {"senderPublicKey", SenderPublicKey.ToBase58()},
                {"assetId", Asset.Id.FromBase58()},
                {"minimalFeeInAssets", Asset.AmountToLong(MinimalFeeInAssets)},
                {"fee", Assets.WAVES.AmountToLong(Fee)},
                {"timestamp", Timestamp.ToLong()}
            };
    }
}

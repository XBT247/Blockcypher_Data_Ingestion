using System.Text.Json.Serialization;

namespace BlockCypher.Application.Blockchain.Dtos
{
    public class BlockDTO
    {
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("chain")]
        public string? Chain { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("fees")]
        public long Fees { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("vsize")]
        public int VSize { get; set; }

        [JsonPropertyName("ver")]
        public int Ver { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("received_time")]
        public string? ReceivedTime { get; set; }

        [JsonPropertyName("coinbase_addr")]
        public string? CoinbaseAddr { get; set; }

        [JsonPropertyName("relayed_by")]
        public string? RelayedBy { get; set; }

        [JsonPropertyName("bits")]
        public int Bits { get; set; }

        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }

        [JsonPropertyName("n_tx")]
        public int NTx { get; set; }

        [JsonPropertyName("prev_block")]
        public string? PrevBlock { get; set; }

        [JsonPropertyName("mrkl_root")]
        public string? MrklRoot { get; set; }

        [JsonPropertyName("txids")]
        public List<string> Txids { get; set; }

        [JsonPropertyName("depth")]
        public int Depth { get; set; }

        [JsonPropertyName("prev_block_url")]
        public string? PrevBlockUrl { get; set; }

        [JsonPropertyName("tx_url")]
        public string? TxUrl { get; set; }

        [JsonPropertyName("next_txids")]
        public object NextTxids { get; set; }
    }
}


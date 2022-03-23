CREATE PARTITION SCHEME [DailyPartitionScheme]
    AS PARTITION [DailyPartitionFunction]
    TO ([PRIMARY], [PRIMARY], [PRIMARY]);


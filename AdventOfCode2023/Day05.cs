using AdventOfCode.Lib;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;

internal class Day05 : PuzzleDayBase
{
    private List<Mapping> _fertilizerToWater = new();
    private List<Mapping> _humidityToLocation = new();
    private List<Mapping> _lightToTemperature = new();
    private List<ValueRange> _seedRanges = new();
    private List<long> _seeds = new();
    private List<Mapping> _seedToSoil = new();
    private List<Mapping> _soilToFertilizer = new();
    private List<Mapping> _temperatureToHumidity = new();
    private List<Mapping> _waterToLight = new();

    public Day05() : base(5)
    {
        ParseInput();
    }

    public override void SolveA()
    {
        List<SeedMap> maps = new();
        foreach (var seed in _seeds)
        {
            SeedMap map = new() { Seed = seed };
            map.Soil = GetMapping(map.Seed, _seedToSoil);
            map.Fertilizer = GetMapping(map.Soil, _soilToFertilizer);
            map.Water = GetMapping(map.Fertilizer, _fertilizerToWater);
            map.Light = GetMapping(map.Water, _waterToLight);
            map.Temperature = GetMapping(map.Light, _lightToTemperature);
            map.Humidity = GetMapping(map.Temperature, _temperatureToHumidity);
            map.Location = GetMapping(map.Humidity, _humidityToLocation);
            maps.Add(map);
        }

        long location = long.MaxValue;
        foreach (var map in maps)
        {
            if (map.Location < location)
                location = map.Location;
        }

        Presentation.ShowAnswer(location);
    }

    public override void SolveB()
    {
        // Tree of ranges
        List<MappedRange>[] mappingTree = new List<MappedRange>[7];

        // For each range in level, check mapping If mapping overlaps, split range and add mapping
        // to tree. Add default mappings for remaining ranges.
        mappingTree[0] = GetMappingB(_seedRanges, _seedToSoil);
        mappingTree[1] = GetMappingB(GetDestinationRanges(mappingTree[0]), _soilToFertilizer);
        mappingTree[2] = GetMappingB(GetDestinationRanges(mappingTree[1]), _fertilizerToWater);
        mappingTree[3] = GetMappingB(GetDestinationRanges(mappingTree[2]), _waterToLight);
        mappingTree[4] = GetMappingB(GetDestinationRanges(mappingTree[3]), _lightToTemperature);
        mappingTree[5] = GetMappingB(GetDestinationRanges(mappingTree[4]), _temperatureToHumidity);
        mappingTree[6] = GetMappingB(GetDestinationRanges(mappingTree[5]), _humidityToLocation);

        // Find the lowest location number
        long minLocation = long.MaxValue;
        foreach (var mappedRange in mappingTree[6])
        {
            long location = mappedRange.Destination.Start;
            if (location <= minLocation)
                minLocation = location;
        }

        Presentation.ShowAnswer(minLocation);
    }

    private List<ValueRange> GetDestinationRanges(List<MappedRange> mappedRanges)
    {
        List<ValueRange> ranges = new();
        foreach (var mappedRange in mappedRanges)
        {
            ranges.Add(mappedRange.Destination);
        }
        return ranges;
    }

    private static long GetMapping(long source, List<Mapping> mappings)
    {
        foreach (var range in mappings)
        {
            if (!range.HasMapping(source))
                continue;
            return range.MapDestination(source);
        }

        return source;
    }

    private static List<MappedRange> GetMappingB(List<ValueRange> source, List<Mapping> mappings)
    {
        List<MappedRange> mappedRanges = new();
        Queue<ValueRange> untested = new(source);
        Queue<ValueRange> unmapped = new();

        // Check all mappings
        foreach (var mapping in mappings)
        {
            while (untested.Count > 0)
            {
                var range = untested.Dequeue();
                if(range.TryIntersect(mapping.SourceRange, out var splitRanges))
                {
                    // Put outliers in unmapped.
                    if (splitRanges.before != null)
                        unmapped.Enqueue(splitRanges.before.Value);
                    if (splitRanges.after != null)
                        unmapped.Enqueue(splitRanges.after.Value);
                    // Add intersection to mapped.
                    var intersection = splitRanges.overlap!.Value;
                    MappedRange mapped = new()
                    {
                        Source = intersection,
                        Destination = mapping.MapDestination(intersection)
                    };
                    mappedRanges.Add(mapped);
                }
                else
                {
                    unmapped.Enqueue(range);
                }
            }
            // move unmapped for next iteration of testing.
            (untested, unmapped) = (unmapped, untested);
        }
        // Give 1-to-1 mappings for remaining ranges.
        while(unmapped.Count > 0)
        {
            var range = untested.Dequeue();
            MappedRange mapped = new()
            {
                Source = range,
                Destination = range
            };
            mappedRanges.Add(mapped);
        }

        return mappedRanges;
    }

    private static void ParseMappings(StringReader reader, List<Mapping> mappingList)
    {
        string? line;
        while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
        {
            string[] parts = line.Split(' ');
            Mapping mapping = new()
            {
                DestinationStart = long.Parse(parts[0]),
                SourceStart = long.Parse(parts[1]),
                Length = long.Parse(parts[2])
            };

            mappingList.Add(mapping);
        }
    }

    private void ParseInput()
    {
        using var reader = Input.GetReader();
        ParseSeeds(reader);

        // Skip past the header
        while (!reader.ReadLine()?.Contains("seed-to-soil map") ?? false) { }
        ParseMappings(reader, _seedToSoil);
        while (!reader.ReadLine()?.Contains("soil-to-fertilizer map") ?? false) { }
        ParseMappings(reader, _soilToFertilizer);
        while (!reader.ReadLine()?.Contains("fertilizer-to-water map") ?? false) { }
        ParseMappings(reader, _fertilizerToWater);
        while (!reader.ReadLine()?.Contains("water-to-light map") ?? false) { }
        ParseMappings(reader, _waterToLight);
        while (!reader.ReadLine()?.Contains("light-to-temperature map") ?? false) { }
        ParseMappings(reader, _lightToTemperature);
        while (!reader.ReadLine()?.Contains("temperature-to-humidity map") ?? false) { }
        ParseMappings(reader, _temperatureToHumidity);
        while (!reader.ReadLine()?.Contains("humidity-to-location map") ?? false) { }
        ParseMappings(reader, _humidityToLocation);
    }

    private void ParseSeeds(StringReader reader)
    {
        string? line = reader.ReadLine();
        if (line == null)
            return;
        string[] seedEntries = line[6..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (var seed in seedEntries)
        {
            _seeds.Add(long.Parse(seed));
        }

        for (int i = 0; i + 1 < seedEntries.Length; i += 2)
        {
            _seedRanges.Add(ValueRange.FromLength(
                start: long.Parse(seedEntries[i]),
                length: long.Parse(seedEntries[i + 1])));
        }
    }

    private readonly struct ValueRange : IEquatable<ValueRange>
    {
        public long End { get; }

        public readonly long Length => End - Start + 1;

        public long Start { get; }

        public ValueRange() : this(0, 0)
        {
        }

        public ValueRange(long start, long end)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (end < start)
                throw new ArgumentOutOfRangeException(nameof(end));
            (Start, End) = (start, end);
        }

        public static ValueRange FromLength(long start, long length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            return new(start, start + length - 1);
        }

        public readonly bool Contains(long number) => number >= Start && number <= End;

        public readonly bool Equals(ValueRange other) => other.Start == Start && other.Length == Length;

        public override readonly bool Equals(object? obj) => obj is ValueRange range && Equals(range);

        public override readonly int GetHashCode() =>  HashCode.Combine(Start, Length);

        public readonly bool Overlaps(ValueRange other) => other.Start <= End && other.End >= Start;

        
        internal bool TryIntersect(ValueRange other, out (ValueRange? before, ValueRange? overlap, ValueRange? after) splitRanges)
        {
            splitRanges = new();
            if (!Overlaps(other))
                return false;

            splitRanges.before = other.Start > Start ? new ValueRange(Start, other.Start - 1) : null;
            splitRanges.overlap = new ValueRange(Math.Max(Start, other.Start), Math.Min(End, other.End));
            splitRanges.after = other.End < End ? new ValueRange(other.End + 1, End) : null;
            return true;
        }
    }

    private class MappedRange
    {
        public ValueRange Source { get; set; }
        public ValueRange Destination { get; set; }
    }

    private class Mapping
    {
        public long DestinationEnd => DestinationStart + Length - 1;
        public long DestinationStart { get; set; }
        public long Length { get; set; }
        public long SourceEnd => SourceStart + Length - 1;
        public long SourceStart { get; set; }

        public ValueRange SourceRange => new(SourceStart, SourceEnd);
        public ValueRange DestinationRange => new(DestinationStart, DestinationEnd);

        public bool HasMapping(long sourceIndex) => sourceIndex >= SourceStart && sourceIndex <= SourceStart + Length;

        public long MapDestination(long source) => DestinationStart + source - SourceStart;

        public ValueRange MapDestination(ValueRange source) => new(
            start: MapDestination(source.Start),
            end: MapDestination(source.End));
    }

    private class SeedMap
    {
        public long Fertilizer { get; set; }
        public long Humidity { get; set; }
        public long Light { get; set; }
        public long Location { get; set; }
        public long Seed { get; set; }
        public long Soil { get; set; }
        public long Temperature { get; set; }
        public long Water { get; set; }
    }
}
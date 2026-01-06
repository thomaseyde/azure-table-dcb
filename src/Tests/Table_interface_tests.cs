using Azure.Data.Tables;
using Core.Tables;
using Core.Tables.Conditions;
using Core.Tables.Fields;
using Core.Testing;

namespace Tests;

public class Table_interface_tests
{
    [Fact]
    public async Task Query_empty_table()
    {
        IDcbTableClient client = new DcbTableClientStub();
        var entities = await client.Query(new All(), []);
        Assert.Empty(entities);
    }

    [Fact]
    public async Task Add_entity()
    {
        IDcbTableClient client = new DcbTableClientStub();
        await client.Add([new TableEntity("partition", "row")]);
        var entities = await client.Query(new All(), []);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task Query_partition()
    {
        var client = new DcbTableClientStub();
        await client.Add([
            new TableEntity("pk-1", "1"),
            new TableEntity("pk-2", "1")
        ]);
        var condition = PartitionKey.Eq("pk-1");
        var entities = await client.Query(condition, []);
        var entity = Assert.Single(entities);
        Assert.Equal("pk-1", entity.PartitionKey);
    }

    [Fact]
    public async Task Query_row()
    {
        var client = new DcbTableClientStub();
        await client.Add([new TableEntity("pk-1", "1")]);
        await client.Add([new TableEntity("pk-1", "2")]);
        var condition = PartitionKey.Eq("pk-1").And(RowKey.Gt("1"));
        var entities = await client.Query(condition, []);
        var entity = Assert.Single(entities);
        Assert.Equal("2", entity.RowKey);
    }
}
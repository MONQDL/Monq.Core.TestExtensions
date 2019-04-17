using Moq;
using RedisEFExtensions.RedisClient;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monq.Tools.TestExtensions.Stubs
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена

    public class StubRedisConnection
    {
        public readonly Dictionary<RedisKey, RedisValue> RedisKeyValueDB = new Dictionary<RedisKey, RedisValue>();
        public readonly Dictionary<RedisKey, Dictionary<RedisValue, RedisValue>> RedisHashTableDb = new Dictionary<RedisKey, Dictionary<RedisValue, RedisValue>>();

        readonly Mock<IDatabase> _moqDb;
        readonly Mock<IRedisConnectionFactory> _moqRedisConFactory;

        public IDatabase Db => _moqDb.Object;
        public IRedisConnectionFactory ConnectionFactory => _moqRedisConFactory.Object;

        public StubRedisConnection()
        {
            _moqDb = new Mock<IDatabase>();

            _moqDb.Setup(x => x.HashDelete(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => RedisHashTableDb.GetValueOrDefault(key)?.Remove(hashField) ?? false);
            _moqDb.Setup(x => x.HashDelete(It.IsAny<RedisKey>(), It.IsAny<RedisValue[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue[], CommandFlags>((key, hashFields, _) =>
                {
                    var i = 0;
                    foreach (var hashField in hashFields)
                    {
                        if (Db.HashDelete(key, hashField)) i++;
                    }
                    return i;
                });
            _moqDb.Setup(x => x.HashDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => Task.FromResult(Db.HashDelete(key, hashField)));
            _moqDb.Setup(x => x.HashDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue[], CommandFlags>((key, hashFields, _) => Task.FromResult(Db.HashDelete(key, hashFields)));
            _moqDb.Setup(x => x.HashExists(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => RedisHashTableDb.GetValueOrDefault(key)?.ContainsKey(hashField) ?? false);
            _moqDb.Setup(x => x.HashExistsAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => Task.FromResult(Db.HashExists(key, hashField)));
            _moqDb.Setup(x => x.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => RedisHashTableDb.GetValueOrDefault(key)?.GetValueOrDefault(hashField) ?? new RedisValue());
            _moqDb.Setup(x => x.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue[], CommandFlags>((key, hashFields, _) => hashFields.Select(x => Db.HashGet(key, x)).Where(x => x.HasValue).ToArray());
            _moqDb.Setup(x => x.HashGetAll(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => RedisHashTableDb.GetValueOrDefault(key)?.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
            _moqDb.Setup(x => x.HashGetAllAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => Task.FromResult(Db.HashGetAll(key)));
            _moqDb.Setup(x => x.HashGetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, CommandFlags>((key, hashField, _) => Task.FromResult(Db.HashGet(key, hashField)));
            _moqDb.Setup(x => x.HashGetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue[], CommandFlags>((key, hashFields, _) => Task.FromResult(Db.HashGet(key, hashFields)));
            _moqDb.Setup(x => x.HashKeys(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
               .Returns<RedisKey, CommandFlags>((key, _) => RedisHashTableDb.GetValueOrDefault(key)?.Keys.ToArray() ?? Enumerable.Empty<RedisValue>().ToArray());
            _moqDb.Setup(x => x.HashKeysAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
               .Returns<RedisKey, CommandFlags>((key, _) => Task.FromResult(Db.HashKeys(key)));

            _moqDb.Setup(x => x.HashSet(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()))
               .Callback<RedisKey, HashEntry[], CommandFlags>((key, hashFields, _) =>
               {
                   if (!RedisHashTableDb.ContainsKey(key))
                       RedisHashTableDb.Add(key, hashFields.ToDictionary(x => x.Name, x => x.Value));
                   else
                       foreach (var hashField in hashFields)
                       {
                           RedisHashTableDb[key][hashField.Name] = hashField.Value;
                       }
               });

            _moqDb.Setup(x => x.HashSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<RedisValue>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, RedisValue, When, CommandFlags>((key, hashField, value, _, __) =>
                {
                    if (!RedisHashTableDb.ContainsKey(key))
                        RedisHashTableDb.Add(key, new Dictionary<RedisValue, RedisValue> { { hashField, value } });
                    else
                        RedisHashTableDb[key][hashField] = value;
                    return true;
                });

            _moqDb.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()))
               .Returns<RedisKey, HashEntry[], CommandFlags>((key, hashFields, _) =>
               {
                   Db.HashSet(key, hashFields);
                   return Task.FromResult(0);
               });

            _moqDb.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<RedisValue>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, RedisValue, When, CommandFlags>((key, hashField, value, _, __) => Task.FromResult(Db.HashSet(key, hashField, value)));

            _moqDb.Setup(x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => RedisKeyValueDB.Remove(key) || RedisHashTableDb.Remove(key));
            _moqDb.Setup(x => x.KeyDelete(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey[], CommandFlags>((keys, _) =>
                {
                    var i = 0;
                    foreach (var key in keys)
                    {
                        if (Db.KeyDelete(key)) i++;
                    }
                    return 0;
                });
            _moqDb.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => Task.FromResult(Db.KeyDelete(key)));
            _moqDb.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey[], CommandFlags>((keys, _) => Task.FromResult(Db.KeyDelete(keys)));

            _moqDb.Setup(x => x.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => RedisKeyValueDB.ContainsKey(key) || RedisHashTableDb.ContainsKey(key));
            _moqDb.Setup(x => x.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => Task.FromResult(Db.KeyExists(key)));

            _moqDb.Setup(x => x.KeyExpire(It.IsAny<RedisKey>(), It.IsAny<TimeSpan?>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, TimeSpan?, CommandFlags>((_, __, ___) => true);

            _moqDb.Setup(x => x.KeyExpire(It.IsAny<RedisKey>(), It.IsAny<DateTime?>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, DateTime?, CommandFlags>((_, __, ___) => true);
            _moqDb.Setup(x => x.KeyExpireAsync(It.IsAny<RedisKey>(), It.IsAny<TimeSpan?>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, TimeSpan?, CommandFlags>((_, __, ___) => Task.FromResult(true));

            _moqDb.Setup(x => x.KeyExpireAsync(It.IsAny<RedisKey>(), It.IsAny<DateTime?>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, DateTime?, CommandFlags>((_, __, ___) => Task.FromResult(true));

            _moqDb.Setup(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => RedisKeyValueDB.GetValueOrDefault(key));
            _moqDb.Setup(x => x.StringGet(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey[], CommandFlags>((keys, _) => keys.Select(key => RedisKeyValueDB.GetValueOrDefault(key)).ToArray());

            _moqDb.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => Task.FromResult(Db.StringGet(key)));
            _moqDb.Setup(x => x.StringGetAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey[], CommandFlags>((keys, _) => Task.FromResult(Db.StringGet(keys)));

            _moqDb.Setup(x => x.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, _, __, ___) =>
                {
                    RedisKeyValueDB[key] = value;
                    return true;
                });
            _moqDb.Setup(x => x.StringSet(It.IsAny<KeyValuePair<RedisKey, RedisValue>[]>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<KeyValuePair<RedisKey, RedisValue>[], When, CommandFlags>((values, _, __) =>
                {
                    foreach (var value in values)
                    {
                        RedisKeyValueDB[value.Key] = value.Value;
                    }
                    return true;
                });

            _moqDb.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, _, __, ___) => Task.FromResult(Db.StringSet(key, value)));
            _moqDb.Setup(x => x.StringSetAsync(It.IsAny<KeyValuePair<RedisKey, RedisValue>[]>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Returns<KeyValuePair<RedisKey, RedisValue>[], When, CommandFlags>((values, _, __) => Task.FromResult(Db.StringSet(values)));

            var moqConnection = new Mock<IConnectionMultiplexer>();
            moqConnection
                .Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns<int, object>((_, __) => _moqDb.Object);

            _moqRedisConFactory = new Mock<IRedisConnectionFactory>();
            _moqRedisConFactory.Setup(x => x.Connection())
                .Returns(moqConnection.Object);
        }
    }
}
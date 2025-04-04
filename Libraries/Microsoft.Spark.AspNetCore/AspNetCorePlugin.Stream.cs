using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Entities;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Extensions;

namespace Microsoft.Spark.AspNetCore;

public partial class AspNetCorePlugin
{
    public class Stream : IStreamer
    {
        public required Func<IActivity, Task<IActivity>> Send { get; set; }
        public event IStreamer.OnChunkHandler OnChunk = (_) => { };

        protected int _index = 0;
        protected string? _id;
        protected string _text = string.Empty;
        protected ChannelData _channelData = new();
        protected List<Attachment> _attachments = [];
        protected List<IEntity> _entities = [];
        protected Queue<IActivity> _queue = [];

        private readonly System.Action _flush;

        public Stream()
        {
            Func<Task> flush = Flush;
            _flush = flush.Debounce();
        }

        public void Emit(MessageActivity activity)
        {
            _queue.Enqueue(activity);
            _flush();
        }

        public void Emit(TypingActivity activity)
        {
            _queue.Enqueue(activity);
            _flush();
        }

        public void Emit(string text)
        {
            Emit(new MessageActivity(text));
        }

        public async Task<MessageActivity> Close()
        {
            while (_id == null || _queue.Count > 0)
            {
                await Task.Delay(200);
            }

            var activity = new MessageActivity(_text)
                .WithId(_id)
                .WithData(_channelData)
                .AddAttachment(_attachments.ToArray())
                .AddEntity(_entities.ToArray())
                .AddStreamFinal();

            var res = await Send(activity).Retry();

            _index = 0;
            _id = null;
            _text = string.Empty;
            _attachments = [];
            _entities = [];
            _channelData = new();

            return (MessageActivity)res;
        }

        protected async Task Flush()
        {
            if (_queue.Count == 0) return;

            while (_queue.TryDequeue(out var activity))
            {
                if (activity is MessageActivity message)
                {
                    _text += message.Text;
                    _attachments.AddRange(message.Attachments ?? []);
                    _entities.AddRange(message.Entities ?? []);
                }

                if (activity.ChannelData != null)
                {
                    _channelData = _channelData.Merge(activity.ChannelData);
                }
            }

            _index++;
            var toSend = new TypingActivity(_text).AddStreamUpdate(_index);

            if (_id != null)
            {
                toSend.WithId(_id);
            }

            var res = await Send(toSend).Retry();

            if (_id == null)
            {
                _id = res.Id;
            }
        }
    }
}
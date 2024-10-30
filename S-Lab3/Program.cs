using System;
using System.Collections.Generic;
using System.Linq;

public class News
{
    public string Title { get; }
    public List<string> Topics { get; }
    public string Content { get; }
    public string VideoUrl { get; }

    public News(string title, List<string> topics, string content = null, string videoUrl = null)
    {
        Title = title;
        Topics = topics;
        Content = content;
        VideoUrl = videoUrl;
    }

    public bool IsTextNews() => !string.IsNullOrEmpty(Content);
    public bool IsVideoNews() => !string.IsNullOrEmpty(VideoUrl);
}

public class Subscriber
{
    public string Name { get; }
    public bool GeneralTextSubscribed { get; private set; }
    public bool VideoSubscribed { get; private set; }
    private List<string> TopicSubscriptions { get; }

    public Subscriber(string name)
    {
        Name = name;
        TopicSubscriptions = new List<string>();
    }

    public void SubscribeToText(string topic = null)
    {
        if (topic == null)
        {
            GeneralTextSubscribed = true;
        }
        else
        {
            TopicSubscriptions.Add(topic);
        }
    }

    public void SubscribeToVideo()
    {
        VideoSubscribed = true;
    }

    public bool ShouldReceive(News news)
    {
        if (news.IsTextNews() && (GeneralTextSubscribed || news.Topics.Any(topic => TopicSubscriptions.Contains(topic))))
        {
            return true;
        }

        if (news.IsVideoNews() && VideoSubscribed)
        {
            return true;
        }

        return false;
    }

    public void Notify(News news)
    {
        Console.WriteLine($"Сповiщення для {Name}: {news.Title}");
    }
}

public class NewsSystem
{
    private List<News> TextNews { get; }
    private List<News> VideoNews { get; }
    private List<Subscriber> Subscribers { get; }

    public NewsSystem()
    {
        TextNews = new List<News>();
        VideoNews = new List<News>();
        Subscribers = new List<Subscriber>();
    }

    public void AddSubscriber(Subscriber subscriber)
    {
        Subscribers.Add(subscriber);
    }

    public void PublishNews(News news)
    {
        if (news.IsTextNews())
        {
            TextNews.Add(news);
        }
        else if (news.IsVideoNews())
        {
            VideoNews.Add(news);
        }

        NotifySubscribers(news);
    }

    private void NotifySubscribers(News news)
    {
        foreach (var subscriber in Subscribers)
        {
            if (subscriber.ShouldReceive(news))
            {
                subscriber.Notify(news);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var newsSystem = new NewsSystem();

        var subscriber1 = new Subscriber("Юра");
        subscriber1.SubscribeToText("Технологiї");
        newsSystem.AddSubscriber(subscriber1);

        var subscriber2 = new Subscriber("Дiма");
        subscriber2.SubscribeToVideo();
        newsSystem.AddSubscriber(subscriber2);

        var textNews = new News("Запуск нових технологiй", new List<string> { "Технологiї" }, "Новiтнi технології світу!");
        var videoNews = new News("Основнi моменти технiчної подiї", new List<string> { "Технологiї" }, videoUrl: "https://video-url.com");

        newsSystem.PublishNews(textNews);
        newsSystem.PublishNews(videoNews);
    }
}
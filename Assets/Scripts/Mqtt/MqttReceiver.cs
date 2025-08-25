using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;

public class MqttReceiver : M2MqttUnityClient
{
    [Header("MQTT topics")]
    [Tooltip("Set the topic to subscribe. !!!ATTENTION!!! multi-level wildcard # subscribes to all topics")]
    public string[] topicSubscribe; // topic to subscribe. !!! The multi-level wildcard # is used to subscribe to all the topics. Attention i if #, subscribe to all topics. Attention if MQTT is on data plan
    [Tooltip("Set the topic to publish (optional)")]
    public string topicPublish = ""; // topic to publish
    public string messagePublish = ""; // message to publish

    [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
    public bool autoTest = false;

    //using C# Property GET/SET and event listener to reduce Update overhead in the controlled objects
    private string m_msg;

    public string msg
    {
        get
        {
            return m_msg;
        }
        set
        {
            if (m_msg == value) return;
            m_msg = value;
            if (OnMessageArrived != null)
            {
                OnMessageArrived(m_msg);
            }
        }
    }

    public event OnMessageArrivedDelegate OnMessageArrived;
    public delegate void OnMessageArrivedDelegate(string newMsg);

    //using C# Property GET/SET and event listener to expose the connection status
    private bool m_isConnected;

    public bool isConnected
    {
        get
        {
            return m_isConnected;
        }
        set
        {
            if (m_isConnected == value) return;
            m_isConnected = value;
            if (OnConnectionSucceeded != null)
            {
                OnConnectionSucceeded(isConnected);
            }
        }
    }
    public event OnConnectionSucceededDelegate OnConnectionSucceeded;
    public delegate void OnConnectionSucceededDelegate(bool isConnected);

    // a list to store the messages
    private List<string> eventMessages = new List<string>();

    public void Publish()
    {
        client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(messagePublish), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("Test message published");
    }
    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        isConnected = true;

        if (autoTest)
        {
            Publish();
        }
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        Debug.Log("Disconnected.");
        isConnected = false;
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("CONNECTION LOST!");
    }

    protected override void SubscribeTopics()
    {
        foreach(string topic in topicSubscribe)
        {
            client.Subscribe(new string[] {topic}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }
    }

    protected override void UnsubscribeTopics()
    {
        foreach(string topic in topicSubscribe)
        {
            client.Unsubscribe(new string[] {topic});
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        //The message is decoded
        msg = System.Text.Encoding.UTF8.GetString(message);

        //Debug.Log("Received: " + msg);
        //Debug.Log("from topic: " + m_msg);

        StoreMessage(msg);
        if (topicSubscribe.Contains(topic))
        {
            if (autoTest)
            {
                autoTest = false;
                Disconnect();
            }
        }
    }

    private void StoreMessage(string eventMsg)
    {
        if (eventMessages.Count > 50)
        {
            eventMessages.Clear();
        }
        eventMessages.Add(eventMsg);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }
}

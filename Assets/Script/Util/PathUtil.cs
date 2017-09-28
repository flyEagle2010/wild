using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
在移动平台中，一般读取资源会通过下面这三个路径：
1.Resources 在打包时，Resources文件夹下的东西会被压缩和加密 一般在Resources下放预制
2.Application.streamingAssetsPath(只读) 需要手动建一个StreamingAssets文件夹。
    在打包时StreamingAssets文件夹中的内容则会原封不动的打入包中。
    StreamingAssets下放二进制文件(csv、bin、txt、xml、json、AB包等);
    不能通过File类来读取这个路径，只能通过WWW类。这是因为在android中，StreamingAssets的东西会被包含在.jar包中(类似于zip压缩文件)。
3.Application.persistentDataPath(可读可写) 除非被引用，否则不会被打包。所以不建议把数据文件放在这个路径

重点说下下面这两个路径：
1.Application.streamingAssetsPath(只读)
需要手动建一个StreamingAssets文件夹。在打包时，Resources文件夹下的东西会被压缩和加密。而StreamingAssets文件夹中的内容则会原封不动的打入包中。
一般在Resources下放预制，StreamingAssets下放二进制文件(csv、bin、txt、xml、json、AB包等)
不能通过File类来读取这个路径，只能通过WWW类。这是因为在android中，StreamingAssets的东西会被包含在.jar包中(类似于zip压缩文件)。

2.Application.persistentDataPath(可读可写)

对于Application.dataPath路径的东西(不包括StreamingAssets和Resources)，除非被引用，否则不会被打包。所以不建议把数据文件放在这个路径。具体的自行打包exe就知道了

ios
Application.dataPath            /var/containers/Bundle/Application/app sandbox/xxx.app/Data 
Application.streamingAssetsPath /var/containers/Bundle/Application/app sandbox/test.app/Data/Raw 
Application.temporaryCachePath /var/mobile/Containers/Data/Application/app sandbox/Library/Caches 
Application.persistentDataPath  /var/mobile/Containers/Data/Application/app sandbox/Documents

android
Application.dataPath            /data/app/package name-1/base.apk 
Application.streamingAssetsPath jar:file:///data/app/package name-1/base.apk!/assets 
Application.temporaryCachePath /storage/emulated/0/Android/data/package name/cache 
Application.persistentDataPath   /storage/emulated/0/Android/data/package name/files
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />

 */
public class PathUtil{
    static string  GetStreamPath()
    {
        string path = "";
#if UNITY_EDITOR
        path = Application.dataPath + "/StreamingAssets/";
#else
        //or sandbox dir.
        path = Application.streamingAssetsPath + "/StreamingAssets/";
#endif
        return path;
    }

    public static string GetExternalPath()
    {
        return Application.persistentDataPath;
    }
}

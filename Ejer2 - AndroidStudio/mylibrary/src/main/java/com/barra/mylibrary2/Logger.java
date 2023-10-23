package com.barra.mylibrary2;

import android.content.Context;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class Logger {
    private static final String LOGTAG = "BarraLOG";
    private static Logger _instance = null;
    private static final String FILENAME = "Logs.txt";

    public static void saveLogs(Context context, String string) {
        try {
            FileOutputStream outputStream = context.openFileOutput(FILENAME, Context.MODE_APPEND);
            String data = string + "\n";
            outputStream.write(data.getBytes());
            outputStream.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static List<String> getLogs(Context context) {
        List<String> strings = new ArrayList<>();
        try {
            File file = new File(context.getFilesDir(), FILENAME);
            if (file.exists()) {
                BufferedReader bufferedReader = new BufferedReader(new FileReader(file));
                String line;
                while ((line = bufferedReader.readLine()) != null) {
                    strings.add(line);
                }
                bufferedReader.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return strings;
    }

    public static void deleteLogs(Context context) {
        File file = new File(context.getFilesDir(), FILENAME);
        if (file.exists()) {
            file.delete();
        }
    }
    public static Logger getInstance(){
        if(_instance == null){
            _instance = new Logger();
        }
        return _instance;
    }

    public String getLogtag(){
        return LOGTAG;
    }
}
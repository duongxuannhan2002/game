using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private string dbFileName = "duaxe.sqlite";
    private string dbPath;

    void Start()
    {
        // Thiết lập đường dẫn cơ sở dữ liệu
        dbPath = Path.Combine(Application.persistentDataPath, dbFileName);
    Debug.Log("start"+ dbPath);
        // Kiểm tra và sao chép cơ sở dữ liệu từ StreamingAssets
        CopyDatabaseIfNotExists();
    }

    private void CopyDatabaseIfNotExists()
{
    if (!File.Exists(dbPath))
    {
        string streamingDbPath = Path.Combine(Application.streamingAssetsPath, dbFileName);

        if (Application.platform == RuntimePlatform.Android)
        {
            CopyDatabaseFromStreamingAssetsToPersistent();
        }
        else
        {
            File.Copy(streamingDbPath, dbPath);
            Debug.Log("Database copied to: " + dbPath);
        }
    }
    else
    {
        Debug.Log("Database already exists at: " + dbPath);
    }
}

    private void CopyDatabaseFromStreamingAssetsToPersistent()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, dbFileName);
        
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            // Đọc file từ stream nếu nó nằm trong file .apk (trên Android)
            WWW reader = new WWW(sourcePath);
            while (!reader.isDone) { }

            // Ghi lại file vào persistentDataPath
            File.WriteAllBytes(dbPath, reader.bytes);
            Debug.Log("Database copied to: " + dbPath);
        }
        else
        {
            // Nếu không thì dùng phương thức bình thường
            File.Copy(sourcePath, dbPath);
        }
    }


    public List<int> GetUnlockedCars(int playerId)
    {
        Debug.Log("Database Path: " + dbPath);
        List<int> unlockedCarIds = new List<int>();

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Car_id FROM PlayerCar WHERE Player_id = @playerId";
                command.Parameters.AddWithValue("@playerId", playerId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        unlockedCarIds.Add(reader.GetInt32(0));
                    }
                }
            }
        }

        return unlockedCarIds;
    }

    public int GetMonney()
    {
        int mon = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Money FROM Player WHERE id = 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mon = reader.GetInt32(0);
                        Debug.Log("tien " + mon);
                    }
                }
            }
        }
        return mon;
    }

    public int GetPrice(int CarId)
    {
        int mon = 0;
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Price FROM Car WHERE id = @CarId";
                command.Parameters.AddWithValue("@CarId", CarId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mon = reader.GetInt32(0);
                    }
                }
            }
        }
        return mon;
    }

    public void AddCarForPlayer(int carId)
    {
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Lệnh SQL thêm dòng mới vào bảng PlayerCar
                command.CommandText = "INSERT INTO PlayerCar (Player_id, Car_id) VALUES (1, @carId)";
                command.Parameters.AddWithValue("@carId", carId);

                try
                {
                    // Thực hiện lệnh SQL
                    command.ExecuteNonQuery();
                    Debug.Log("Đã thêm xe mới cho người chơi: ");
                }
                catch (SqliteException e)
                {
                    Debug.LogError("Lỗi khi thêm dữ liệu: " + e.Message);
                }
            }
        }
    }
    public void UpdateMoney(int Money)
    {
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Lệnh SQL thêm dòng mới vào bảng PlayerCar
                command.CommandText = "UPDATE Player Set Money =  @money Where id = 1";
                command.Parameters.AddWithValue("@money", Money);

                try
                {
                    // Thực hiện lệnh SQL
                    command.ExecuteNonQuery();
                    Debug.Log("Đã thêm xe mới cho người chơi: ");
                }
                catch (SqliteException e)
                {
                    Debug.LogError("Lỗi khi thêm dữ liệu: " + e.Message);
                }
            }
        }
    }
    public List<int> GetMapUnlock(int playerId)
    {
        List<int> mapUnlock = new List<int>();

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT Map_id FROM PlayerMap WHERE Player_id = @playerId";
                command.Parameters.AddWithValue("@playerId", playerId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mapUnlock.Add(reader.GetInt32(0));
                    }
                }
            }
        }

        return mapUnlock;
    }
    public void UpdateAddMoney()
    {
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Lệnh SQL thêm dòng mới vào bảng PlayerCar
                command.CommandText = "UPDATE Player Set Money = Money + 10 Where id = 1";
                try
                {
                    // Thực hiện lệnh SQL
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Debug.LogError("Lỗi khi thêm dữ liệu: " + e.Message);
                }
            }
        }
    }
    public void UpdateChallenge(int idmap, int tt)
    {
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Lệnh SQL thêm dòng mới vào bảng PlayerCar
                if (tt == 1)
                {
                    command.CommandText = "UPDATE Map Set Tt1 = 1 Where id = @idmap";

                }else{
                    command.CommandText = "UPDATE Map Set Tt2 = 1 Where id = @idmap";
                }
                try
                {
                    // Thực hiện lệnh SQL
                    command.Parameters.AddWithValue("@idmap", idmap);
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Debug.LogError("Lỗi khi thêm dữ liệu: " + e.Message);
                }
            }
        }
    }

    public List<int> GetChallenge(int id)
    {
        List<int> tt = new List<int>();

        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                command.CommandText = "SELECT tt1, tt2 FROM Map WHERE id = @idmap";
                command.Parameters.AddWithValue("@idmap", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tt.Add(reader.GetInt32(0));  // Adding value from column tt1
                        tt.Add(reader.GetInt32(1));  // Adding value from column tt2
                    }
                }
            }
        }
        // Debug.Log("trong csdl"+ tt[0] + "" + tt[1]);

        return tt;
    }
    public void unlockMap(int id)
    {
        using (var connection = new SqliteConnection("URI=file:" + dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Query to get the list of unlocked cars for the player
                try
                {
                    // Thực hiện lệnh SQL
                    command.CommandText = "INSERT INTO PlayerMap(Player_id, Map_id) VALUES(1, @idmap)";
                    command.Parameters.AddWithValue("@idmap", id);
                    // command.Parameters.AddWithValue("@tt", tt);
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Debug.LogError("Lỗi khi thêm dữ liệu: " + e.Message);
                }
            }
        }
    }
}
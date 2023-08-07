﻿using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public class TodayImageStorage : ITodayImageStorage
{
    private readonly IPreferenceStorage preferenceStorage;

    public TodayImageStorage(IPreferenceStorage preferenceStorage)
    {
        this.preferenceStorage = preferenceStorage;
    }

    public static readonly string FullStartDateKey = nameof(TodayImageStorage) +
        "." + nameof(TodayImage.FullStartDate);

    public static readonly string ExpiresAtKey =
        nameof(TodayImageStorage) + "." + nameof(TodayImage.ExpiresAt);

    public static readonly string CopyrightKey =
        nameof(TodayImageStorage) + "." + nameof(TodayImage.Copyright);

    public static readonly string CopyrightLinkKey = nameof(TodayImageStorage) +
        "." + nameof(TodayImage.CopyrightLink);

    public const string FullStartDateDefault = "201901010700";

    public static readonly DateTime ExpiresAtDefault = new(2019, 1, 2, 7, 0, 0);

    public const string CopyrightDefault =
        "Salt field province vietnam work (© Quangpraha/Pixabay)";

    public const string CopyrightLinkDefault =
        "https://pixabay.com/photos/salt-field-province-vietnam-work-3344508/";

    public const string FileName = "todayImage.bin";

    public static readonly string TodayImagePath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), FileName);

    public async Task<TodayImage>
        GetTodayImageAsync(bool includingImageStream)
    {
        var todayImage = new TodayImage
        {
            FullStartDate =
                preferenceStorage.Get(FullStartDateKey, FullStartDateDefault),
            ExpiresAt = preferenceStorage.Get(ExpiresAtKey, ExpiresAtDefault),
            Copyright = preferenceStorage.Get(CopyrightKey, CopyrightDefault),
            CopyrightLink = preferenceStorage.Get(CopyrightLinkKey,
                CopyrightLinkDefault)
        };

        if (!File.Exists(TodayImagePath))
        {
            await using var imageAssetFileStream =
                new FileStream(TodayImagePath, FileMode.Create) ??
                throw new NullReferenceException("Null file stream.");
            await using var imageAssetStream = typeof(TodayImageStorage).Assembly.GetManifestResourceStream(FileName) ??
                throw new NullReferenceException(
                    "Null manifest resource stream");
            await imageAssetStream.CopyToAsync(imageAssetFileStream);
        }

        if (!includingImageStream)
        {
            return todayImage;
        }

        var imageMemoryStream = new MemoryStream();
        await using var imageFileStream = new FileStream(TodayImagePath, FileMode.Open);
        await imageFileStream.CopyToAsync(imageMemoryStream);
        todayImage.ImageBytes = imageMemoryStream.ToArray();

        return todayImage;
    }

    public async Task SaveTodayImageAsync(TodayImage todayImage,
        bool savingExpiresAtOnly)
    {
        preferenceStorage.Set(ExpiresAtKey, todayImage.ExpiresAt);
        if (savingExpiresAtOnly)
        {
            return;
        }

        if (todayImage.ImageBytes == null)
        {
            throw new ArgumentException($"Null image bytes.",
                nameof(todayImage));
        }

        preferenceStorage.Set(FullStartDateKey, todayImage.FullStartDate);
        preferenceStorage.Set(CopyrightKey, todayImage.Copyright);
        preferenceStorage.Set(CopyrightLinkKey, todayImage.CopyrightLink);

        await using var imageFileStream =
            new FileStream(TodayImagePath, FileMode.Create);
        await imageFileStream.WriteAsync(todayImage.ImageBytes, 0,
            todayImage.ImageBytes.Length);
    }
}

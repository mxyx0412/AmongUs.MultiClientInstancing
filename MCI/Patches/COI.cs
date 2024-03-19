using BepInEx.Configuration;
using System;
using UnityEngine;

namespace MCI.Patches.ClientOptions;

public sealed class ClientOptionItem : ClientActionItem
{
    public ConfigEntry<bool> Config { get; private set; }

    private ClientOptionItem(
        string name,
        ConfigEntry<bool> config,
        OptionsMenuBehaviour optionsMenuBehaviour)
        : base(
            name,
            optionsMenuBehaviour)
    {
        Config = config;
        UpdateToggle();
    }

    /// <summary>
    /// Modオプション画面にconfigのトグルを追加します
    /// </summary>
    /// <param name="name">ボタンラベルの翻訳キーとボタンのオブジェクト名</param>
    /// <param name="config">対応するconfig</param>
    /// <param name="optionsMenuBehaviour">OptionsMenuBehaviourのインスタンス</param>
    /// <param name="additionalOnClickAction">クリック時に追加で発火するアクション．configが変更されたあとに呼ばれる</param>
    /// <returns>作成したアイテム</returns>
    public static ClientOptionItem Create(
        string name,
        ConfigEntry<bool> config,
        OptionsMenuBehaviour optionsMenuBehaviour,
        Action additionalOnClickAction = null)
    {
        var item = new ClientOptionItem(name, config, optionsMenuBehaviour);
        item.OnClickAction = () =>
        {
            config.Value = !config.Value;
            item.UpdateToggle();
            additionalOnClickAction?.Invoke();
        };
        return item;
    }

    public void UpdateToggle()
    {
        if (ToggleButton == null) return;

        var color = Config.Value ? new Color32(255, 129, 166, byte.MaxValue) : new Color32(77, 77, 77, byte.MaxValue);
        ToggleButton.Background.color = color;
        ToggleButton.Rollover?.ChangeOutColor(color);
    }
}
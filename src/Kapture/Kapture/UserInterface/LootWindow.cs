using System.Linq;
using System.Numerics;

using CheapLoc;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;

namespace Kapture
{
    /// <inheritdoc />
    public class LootWindow : PluginWindow
    {
        private readonly IKapturePlugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="LootWindow"/> class.
        /// </summary>
        /// <param name="plugin">plugin.</param>
        public LootWindow(KapturePlugin plugin)
            : base(plugin, Loc.Localize("LootOverlayWindow", "Loot") + "###Kapture_Loot_Window", ImGuiWindowFlags.NoFocusOnAppearing)
        {
            this.plugin = plugin;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            ImGui.SetNextWindowSize(new Vector2(380 * ImGuiHelpers.GlobalScale, 220 * ImGuiHelpers.GlobalScale), ImGuiCond.FirstUseEver);
            if (this.plugin.Configuration.Enabled)
            {
                var lootEvents = this.plugin.LootEvents.ToList();
                if (lootEvents.Count > 0)
                {
                    var col1 = 200f * ImGuiHelpers.GlobalScale;
                    var col2 = 270f * ImGuiHelpers.GlobalScale;

                    ImGui.TextColored(ImGuiColors.DalamudViolet, Loc.Localize("LootItemName", "Item"));
                    ImGui.SameLine(col1);
                    ImGui.TextColored(ImGuiColors.DalamudViolet, Loc.Localize("LootEventType", "Event"));
                    ImGui.SameLine(col2);
                    ImGui.TextColored(ImGuiColors.DalamudViolet, Loc.Localize("LootPlayer", "Player"));
                    ImGui.Separator();

                    foreach (var lootEvent in lootEvents)
                    {
                        ImGui.Text(lootEvent.ItemNameAbbreviated);
                        ImGui.SameLine(col1);
                        ImGui.Text(lootEvent.LootEventTypeName);
                        ImGui.SameLine(col2);
                        ImGui.Text(lootEvent.PlayerName);
                        ImGui.NextColumn();
                    }
                }
                else
                {
                    ImGui.Text(Loc.Localize("WaitingForItems", "Waiting for items."));
                }
            }
        }

        /// <summary>
        /// Checks whether should display loot window.
        /// </summary>
        /// <returns>indicator whether to show loot window.</returns>
        public bool ShowLootWindow()
        {
            return !this.plugin.IsInitializing && this.plugin.IsLoggedIn() && this.plugin.Configuration.Enabled &&
                   (this.plugin.Configuration.LootDisplayMode == DisplayMode.AlwaysOn.Code ||
                    (this.plugin.Configuration.LootDisplayMode == DisplayMode.ContentOnly.Code && this.plugin.InContent) ||
                    (this.plugin.Configuration.LootDisplayMode == DisplayMode.DuringRollsOnly.Code && this.plugin.IsRolling));
        }
    }
}

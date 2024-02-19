using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MyBlazorApp.Data;

namespace MyBlazorApp.Services
{
    public class PresetService
    {
        private IJSRuntime _jsRuntime;
        private Dictionary<string, Preset> _presets;

        public PresetService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _presets = new Dictionary<string, Preset>
            {
                { "Perf1", new Preset() },
                { "Perf2", new Preset() },
                { "Perf3", new Preset() },
            };
        }

        public void UpdatePreset(string presetName, int weight, int calorieIntake, int height)
        {
            _presets[presetName] = new Preset { Weight = weight, CalorieIntake = calorieIntake, Height = height };
        }

        public async Task SavePresets()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "presets", JsonSerializer.Serialize(_presets));
        }

        public async Task LoadPresets()
        {
            var savedPresets = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "presets");
            if (!string.IsNullOrEmpty(savedPresets))
            {
                _presets = JsonSerializer.Deserialize<Dictionary<string, Preset>>(savedPresets);
            }
        }
    }
}
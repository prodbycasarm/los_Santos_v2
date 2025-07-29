 <h1>Los Santos Custom Workshop</h1>
  <p>
    <strong>Los Santos Custom Workshop</strong> is a GTA V mod that brings a fully-featured, in-game vehicle modification menu inspired by Los Santos Customs. It allows you to upgrade, repair, and customize your vehicles with a wide range of options, all accessible from designated upgrade zones on the map.
  </p>

  <div class="section">
    <h2>Features</h2>
    <ul>
      <li>Upgrade and repair vehicles at custom workshop zones</li>
      <li>Change primary, secondary, interior, wheel, and dial colors</li>
      <li>Apply metallic, matte, chrome, metal, and pearlescent finishes</li>
      <li>Install and remove neons, turbo, xenon headlights, and more</li>
      <li>Upgrade brakes, transmission, suspension, engine, and armor</li>
      <li>Open/close individual doors, trunk, and hood</li>
      <li>Preview and purchase different wheel types and rim styles</li>
      <li>Custom blip icons and names for workshop locations</li>
      <li>Money system integration for realistic upgrade costs</li>
      <li>Easy-to-use menu powered by LemonUI</li>
    </ul>
  </div>

  <div class="section">
    <h2>Requirements</h2>
    <ul>
      <li>Grand Theft Auto V (PC)</li>
      <li><a href="https://github.com/crosire/scripthookvdotnet">ScriptHookVDotNet</a></li>
      <li><a href="https://github.com/LemonUIbyLemon/LemonUI">LemonUI</a> (for menu system)</li>
      <li>.NET Framework 4.8</li>
    </ul>
  </div>

  <div class="section">
    <h2>Installation</h2>
    <ol>
      <li>Install <strong>ScriptHookV</strong> and <strong>ScriptHookVDotNet</strong> in your GTA V directory.</li>
      <li>Download and install <strong>LemonUI</strong> (place LemonUI .dll files in the <code>scripts</code> folder).</li>
      <li>Place <code>los_Santos_v2.dll</code> and <code>config_ls2.ini</code> in your <code>scripts</code> folder.</li>
      <li>Ensure your <code>scripts</code> folder is in the root of your GTA V directory.</li>
      <li>Launch GTA V. The mod will load automatically.</li>
    </ol>
  </div>

  <div class="section">
    <h2>Usage</h2>
    <ol>
      <li>Drive to any upgrade zone (see map blips for locations).</li>
      <li>When prompted, press the configured key (default: <strong>Enter</strong>) to open the workshop menu.</li>
      <li>Navigate the menu to select upgrades, colors, and modifications.</li>
      <li>Upgrades cost in-game money; ensure you have enough funds.</li>
      <li>Press the menu key again to close the workshop.</li>
    </ol>
    <p>
      <strong>Note:</strong> All upgrades and repairs require you to be inside a vehicle.
    </p>
  </div>

  <div class="section">
    <h2>Configuration</h2>
    <ul>
      <li>Edit <code>config_ls2.ini</code> to change the workshop key, blip appearance, and upgrade zone locations.</li>
      <li>Example config options:
        <pre>
[Options]
Button=Enter

[Blip]
Sprite=LosSantosCustoms
Color=BlueLight
Name=Custom Workshop

[UpgradeZone1]
LocationX=...
LocationY=...
LocationZ=...
        </pre>
      </li>
    </ul>
  </div>

  <div class="section">
    <h2>Credits</h2>
    <ul>
      <li>Script by <strong>casarm</strong></li>
      <li>Powered by <a href="https://github.com/LemonUIbyLemon/LemonUI">LemonUI</a> and <a href="https://github.com/crosire/scripthookvdotnet">ScriptHookVDotNet</a></li>
    </ul>
  </div>

  <div class="section">
    <h2>License</h2>
    <p>
      This mod is provided as-is for personal use. Do not redistribute without permission.
    </p>
  </div>

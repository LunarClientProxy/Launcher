@echo off
echo You can close this windows after Lunar Client started!
C:\Users\User\.lunarclient\jre\zulu17.30.15-ca-fx-jre17.0.1-win_x64\bin\javaw.exe --add-modules jdk.naming.dns --add-exports jdk.naming.dns/com.sun.jndi.dns=java.naming -Djna.boot.library.path=C:\Users\User\.lunarclient\offline\1.8\natives --add-opens java.base/java.io=ALL-UNNAMED -Xms3G -Xmx3G -Xmn1G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M -Djava.library.path=C:\Users\User\.lunarclient\offline\1.8\natives -XX:+DisableAttachMechanism -cp C:\Users\User\.lunarclient\offline\1.8\lunar-assets-prod-1-optifine.jar;C:\Users\User\.lunarclient\offline\1.8\lunar-assets-prod-2-optifine.jar;C:\Users\User\.lunarclient\offline\1.8\lunar-assets-prod-3-optifine.jar;C:\Users\User\.lunarclient\offline\1.8\lunar-libs.jar;C:\Users\User\.lunarclient\offline\1.8\lunar-prod-optifine.jar;C:\Users\User\.lunarclient\offline\1.8\OptiFine.jar;C:\Users\User\.lunarclient\offline\1.8\vpatcher-prod.jar com.moonsworth.lunar.patcher.LunarMain --version 1.8 --accessToken 0 --assetIndex 1.8 --userProperties {} --gameDir C:\Users\User\AppData\Roaming\.minecraft --width 854 --height 480 --texturesDir C:\Users\User\.lunarclient\textures --assetsDir C:\Users\User\AppData\Roaming\.minecraft\assets
echo You can close this window after Lunar Client started!
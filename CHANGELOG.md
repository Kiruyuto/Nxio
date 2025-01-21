# Changelog

## 1.0.0 (2025-01-21)


### ⚠ BREAKING CHANGES

* **bot:** Rename `Nxio.App` to `Nxio.Bot`

### 🚀 Features & Enhancements

* Add basic CI & repo config ([cef1187](https://github.com/Kiruyuto/Nxio/commit/cef118761d1c27bc05c8498e4e5ab17732875f84))
* Add mute background service ([9b9e1d8](https://github.com/Kiruyuto/Nxio/commit/9b9e1d8b9bc37b374d945d32bd88e5a64d3e7860))
* **bot:** Add `User Stats` ([aee42dd](https://github.com/Kiruyuto/Nxio/commit/aee42ddc4c907237d6a9db4ef50d2448316f1149))
* **bot:** Add basic user upgrades logic ([eff39b1](https://github.com/Kiruyuto/Nxio/commit/eff39b15b53b309e91bf540d4d1f48f5ee5ba1aa))
* **bot:** Add coin module; Coin count ([ed431a2](https://github.com/Kiruyuto/Nxio/commit/ed431a293df025244674a37793c064840569d533))
* **bot:** Add DB and initial "coin reaction" handler ([93f041d](https://github.com/Kiruyuto/Nxio/commit/93f041db4b652cdf238fb75b6556df8071ce16e6))
* **bot:** Add informational `ready` event handler ([4368c2c](https://github.com/Kiruyuto/Nxio/commit/4368c2c8e04108e0c8d29e16673be3b3ddce906c))
* **bot:** Add mute text command & "Unmute" background service ([#9](https://github.com/Kiruyuto/Nxio/issues/9)) ([a35d8f2](https://github.com/Kiruyuto/Nxio/commit/a35d8f2f3086e6572d175fd4ee8d60a55a6cad81))
* **bot:** Add option to list muted users ([2f1918a](https://github.com/Kiruyuto/Nxio/commit/2f1918a83094694a03ba3b27d0fc2d83b7ea80d7))
* **bot:** Add roulette module ([4f591c7](https://github.com/Kiruyuto/Nxio/commit/4f591c773acd8d865453bb7ed038706d8fa67fa2))
* **bot:** Add TEXT version of `/Roulette` ([a6c5f98](https://github.com/Kiruyuto/Nxio/commit/a6c5f98ed733e718966b81bc6213723df9b938d4))
* **bot:** Add UserMutes ([b004565](https://github.com/Kiruyuto/Nxio/commit/b004565874fd653df014d470a58485224c4d3e41))
* **bot:** Fix validation within mute command ([29363b6](https://github.com/Kiruyuto/Nxio/commit/29363b618d4ffb0b7f8448b595fa00ff82ccb985))
* **bot:** Init guild settings ([8a8583e](https://github.com/Kiruyuto/Nxio/commit/8a8583e21ac60e9bdcfbf5d4928b1202deb6da48))
* **bot:** Init project ([af61a3c](https://github.com/Kiruyuto/Nxio/commit/af61a3ccb2e6e378d72d02cffc8552deb6c649dc))
* **bot:** Return failed slash commands as ephemeral messages ([8253bb0](https://github.com/Kiruyuto/Nxio/commit/8253bb010b4c3cbf8054ed4ba4027c16e9d51895))
* **bot:** Use static anonymous functions ([277bd62](https://github.com/Kiruyuto/Nxio/commit/277bd62871fcde279f273d4d53c1df7d99d2a89a))
* **ci:** Add GitHub `CodeQL` action ([631f94b](https://github.com/Kiruyuto/Nxio/commit/631f94be64b79255845f5390761374ab6601197d))
* **ci:** Add release pipeline ([44c46ea](https://github.com/Kiruyuto/Nxio/commit/44c46ea6b5617f2681099e44e8b4ebfb40b65539))
* **ci:** Add resharper code inspection ([e634cc8](https://github.com/Kiruyuto/Nxio/commit/e634cc8689838a8d2e7b21e5495eb862f4213cfa))
* **ci:** Use latest GH Actions runners ([#8](https://github.com/Kiruyuto/Nxio/issues/8)) ([88bb628](https://github.com/Kiruyuto/Nxio/commit/88bb6283937b9e0eea544ebb3ccf26eac6a1e9f0))
* **core:** Add sql server & db docker container ([2259b54](https://github.com/Kiruyuto/Nxio/commit/2259b54d9c2728ec39c4de985db6788e7679c40e))
* **docs:** Init project readme ([06e7c63](https://github.com/Kiruyuto/Nxio/commit/06e7c638cd8af929546b464fca7853e741849249))


### 🐛 Bug Fixes

* **bot:** Add missing implicit using annotation ([6d57554](https://github.com/Kiruyuto/Nxio/commit/6d5755426575325b60c338a73444840ee5f3a64f))
* **bot:** EF virtual classes with no inheritor warns ([eee0708](https://github.com/Kiruyuto/Nxio/commit/eee070879ddd8edc1d597118c0b854a9a1694f36))
* **bot:** Implicit using ([83d3b32](https://github.com/Kiruyuto/Nxio/commit/83d3b32672edc4abeb6c5c067da154c004b815fa))
* **bot:** Init properties warns on EF entities ([f2cad41](https://github.com/Kiruyuto/Nxio/commit/f2cad411fb3bf1f969e0e60b4779720d53083dbf))
* **bot:** Mark user as optional ([ccfd5c0](https://github.com/Kiruyuto/Nxio/commit/ccfd5c02e62c8ce9b0f0fc81a86094ab1734b1bc))
* **ci:** Do not trigger release every push ([3adab63](https://github.com/Kiruyuto/Nxio/commit/3adab633d50f26a24ea6a4fa59350de650760a70))
* **ci:** Fix `.csproj` paths ([fa47f95](https://github.com/Kiruyuto/Nxio/commit/fa47f95d20717d523768b49bf1642eb9362ea78c))
* **ci:** Replace `app` & `app name` with `bot` ([628cd0a](https://github.com/Kiruyuto/Nxio/commit/628cd0a63bfce23cb1f81114717811b0a5162a60))
* **ci:** Use full verbosity names ([2fc011a](https://github.com/Kiruyuto/Nxio/commit/2fc011a4c5623ce9950cbe0a38a98c501be88f56))


### 🏡 Chore

* Allow code quality rule violation for `TypeEnum` ([cd9abd6](https://github.com/Kiruyuto/Nxio/commit/cd9abd6bcbcb095354a88e8b9b15444ecbdac6d7))
* **bot:** Add time validation; Remove redundant try-catch ([1119a8f](https://github.com/Kiruyuto/Nxio/commit/1119a8fb6c67d2eaaa272fbe9aa17e20548ab30b))
* **bot:** Do not set flags on non-ephemeral slash commands response ([219921a](https://github.com/Kiruyuto/Nxio/commit/219921a1eb8967b827303b258c69592c7dd7db2d))
* **bot:** Make text commands text insensitive ([c040623](https://github.com/Kiruyuto/Nxio/commit/c0406239541bc8a48b9792a6ca6691f23bd3ea8b))
* **bot:** Rename `Nxio.App` to `Nxio.Bot` ([fe13894](https://github.com/Kiruyuto/Nxio/commit/fe13894024702c67f38dc28862aca5a7011362c1))
* **bot:** Use ephemeral response; Adjust embed style ([2c5594a](https://github.com/Kiruyuto/Nxio/commit/2c5594a4c2c6e84faa8f951260940c9dee8e30ac))
* **bot:** Use guild-only commands, use global commands ([4e878af](https://github.com/Kiruyuto/Nxio/commit/4e878afabff6564199f075d2804ae0c123c16973))
* **ci:** Adjust `release-please` config ([ab22fae](https://github.com/Kiruyuto/Nxio/commit/ab22fae8dee5464aecdff0dc030e2b1b5c651c89))
* **ci:** Adjust build step verbosity ([a0f1024](https://github.com/Kiruyuto/Nxio/commit/a0f102455e92204269ee6ab03191b5c9625b7e71))
* **ci:** Remove concurrency group ([bc0a2a6](https://github.com/Kiruyuto/Nxio/commit/bc0a2a608f9cb023869039a1af7bcf36aac56bc3))
* **ci:** Rename workflow ([da2922f](https://github.com/Kiruyuto/Nxio/commit/da2922f4a4fbefb98f7dbc43e286ac1cf14c0e7e))
* **ci:** Use full argument names ([6819b7e](https://github.com/Kiruyuto/Nxio/commit/6819b7e2a5aa854b024aeb85a6c3729517eb939b))
* **deps:** Update all non-major dependencies ([#7](https://github.com/Kiruyuto/Nxio/issues/7)) ([8284399](https://github.com/Kiruyuto/Nxio/commit/8284399d89aae601b9da37015bd85413f9cf1452))
* **deps:** Update all non-major dependencies to 1.0.0-alpha.334 ([#6](https://github.com/Kiruyuto/Nxio/issues/6)) ([678111f](https://github.com/Kiruyuto/Nxio/commit/678111faacf7d974166d284b8a1c316cd93df225))
* **deps:** Update all non-major dependencies to 1.0.0-alpha.339 ([#10](https://github.com/Kiruyuto/Nxio/issues/10)) ([51a2424](https://github.com/Kiruyuto/Nxio/commit/51a24242d16b952a5c16beaa2caae2c0fe90f855))
* **deps:** Update all non-major dependencies to 9.0.1 ([#12](https://github.com/Kiruyuto/Nxio/issues/12)) ([53a0e95](https://github.com/Kiruyuto/Nxio/commit/53a0e954752ba7e8757d6578ccb572c6a1e45eca))
* **deps:** Update dependency SonarAnalyzer.CSharp to 10.5.0.109200 ([#11](https://github.com/Kiruyuto/Nxio/issues/11)) ([df093e2](https://github.com/Kiruyuto/Nxio/commit/df093e2b6e85173ec146fa1cc28702184e78e644))


### 🎨 Style

* **bot:** Format code ([32ab933](https://github.com/Kiruyuto/Nxio/commit/32ab933b4bd08810211b1c8ca8ca13c91667dcd7))
* **bot:** Move 'upgrades' to `ShopModule` ([c0dfb9c](https://github.com/Kiruyuto/Nxio/commit/c0dfb9c5ade8d34fdf826d8e5969ab4ce5ad9770))
* **bot:** Prettify embed message ([e4500a4](https://github.com/Kiruyuto/Nxio/commit/e4500a449e0f2b4190d0f071275e0f679c4e9c2b))
* **bot:** Run `dotnet-format` again after saving the files ([2b2d7b0](https://github.com/Kiruyuto/Nxio/commit/2b2d7b0f62c3017c74288c3a20f6bea00ee0d694))
* **bot:** Run dotnet-format ([8eedfc2](https://github.com/Kiruyuto/Nxio/commit/8eedfc25aa56d4a54e5229ec2b6d642aa0922cb4))
* **core:** Adjust editorconfig to eclude migrations dir ([c6296a8](https://github.com/Kiruyuto/Nxio/commit/c6296a8f0b07d8575679675eb89539739aa93d32))


### 💅 Refactors

* **bot:** Use dbContext DI directly as modules are transient ([37931fc](https://github.com/Kiruyuto/Nxio/commit/37931fcfb701f293948769dc1cd5ccf940b1c4ee))

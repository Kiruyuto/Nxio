# Changelog

## 1.0.0 (2024-12-02)


### ⚠ BREAKING CHANGES

* **bot:** Rename `Nxio.App` to `Nxio.Bot`

### 🚀 Features & Enhancements

* Add basic CI & repo config ([cef1187](https://github.com/Kiruyuto/Nxio/commit/cef118761d1c27bc05c8498e4e5ab17732875f84))
* **bot:** Add basic user upgrades logic ([eff39b1](https://github.com/Kiruyuto/Nxio/commit/eff39b15b53b309e91bf540d4d1f48f5ee5ba1aa))
* **bot:** Add coin module; Coin count ([ed431a2](https://github.com/Kiruyuto/Nxio/commit/ed431a293df025244674a37793c064840569d533))
* **bot:** Add DB and initial "coin reaction" handler ([93f041d](https://github.com/Kiruyuto/Nxio/commit/93f041db4b652cdf238fb75b6556df8071ce16e6))
* **bot:** Add option to list muted users ([2f1918a](https://github.com/Kiruyuto/Nxio/commit/2f1918a83094694a03ba3b27d0fc2d83b7ea80d7))
* **bot:** Add roulette module ([4f591c7](https://github.com/Kiruyuto/Nxio/commit/4f591c773acd8d865453bb7ed038706d8fa67fa2))
* **bot:** Init project ([af61a3c](https://github.com/Kiruyuto/Nxio/commit/af61a3ccb2e6e378d72d02cffc8552deb6c649dc))
* **ci:** Add release pipeline ([44c46ea](https://github.com/Kiruyuto/Nxio/commit/44c46ea6b5617f2681099e44e8b4ebfb40b65539))
* **core:** Add sql server & db docker container ([2259b54](https://github.com/Kiruyuto/Nxio/commit/2259b54d9c2728ec39c4de985db6788e7679c40e))


### 🐛 Bug Fixes

* **bot:** Mark user as optional ([ccfd5c0](https://github.com/Kiruyuto/Nxio/commit/ccfd5c02e62c8ce9b0f0fc81a86094ab1734b1bc))
* **ci:** Do not trigger release every push ([3adab63](https://github.com/Kiruyuto/Nxio/commit/3adab633d50f26a24ea6a4fa59350de650760a70))
* **ci:** Fix `.csproj` paths ([fa47f95](https://github.com/Kiruyuto/Nxio/commit/fa47f95d20717d523768b49bf1642eb9362ea78c))
* **ci:** Replace `app` & `app name` with `bot` ([628cd0a](https://github.com/Kiruyuto/Nxio/commit/628cd0a63bfce23cb1f81114717811b0a5162a60))
* **ci:** Use full verbosity names ([2fc011a](https://github.com/Kiruyuto/Nxio/commit/2fc011a4c5623ce9950cbe0a38a98c501be88f56))


### 🏡 Chore

* **bot:** Rename `Nxio.App` to `Nxio.Bot` ([fe13894](https://github.com/Kiruyuto/Nxio/commit/fe13894024702c67f38dc28862aca5a7011362c1))
* **bot:** Use ephemeral response; Adjust embed style ([2c5594a](https://github.com/Kiruyuto/Nxio/commit/2c5594a4c2c6e84faa8f951260940c9dee8e30ac))
* **bot:** Use guild-only commands, use global commands ([4e878af](https://github.com/Kiruyuto/Nxio/commit/4e878afabff6564199f075d2804ae0c123c16973))
* **ci:** Adjust `release-please` config ([ab22fae](https://github.com/Kiruyuto/Nxio/commit/ab22fae8dee5464aecdff0dc030e2b1b5c651c89))
* **ci:** Adjust build step verbosity ([a0f1024](https://github.com/Kiruyuto/Nxio/commit/a0f102455e92204269ee6ab03191b5c9625b7e71))
* **ci:** Use full argument names ([6819b7e](https://github.com/Kiruyuto/Nxio/commit/6819b7e2a5aa854b024aeb85a6c3729517eb939b))


### 🎨 Style

* **bot:** Format code ([32ab933](https://github.com/Kiruyuto/Nxio/commit/32ab933b4bd08810211b1c8ca8ca13c91667dcd7))
* **bot:** Prettify embed message ([e4500a4](https://github.com/Kiruyuto/Nxio/commit/e4500a449e0f2b4190d0f071275e0f679c4e9c2b))
* **core:** Adjust editorconfig to eclude migrations dir ([c6296a8](https://github.com/Kiruyuto/Nxio/commit/c6296a8f0b07d8575679675eb89539739aa93d32))


### 💅 Refactors

* **bot:** Use dbContext DI directly as modules are transient ([37931fc](https://github.com/Kiruyuto/Nxio/commit/37931fcfb701f293948769dc1cd5ccf940b1c4ee))

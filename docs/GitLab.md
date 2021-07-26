
### 重新加载配置信息
```
$ docker exec -it gitlab gitlab-ctl reconfigure
```
### 备份
```
db (数据库)
uploads (附件)
repositories (代码库)
builds (CI作业输出日志)
artifacts (CI工件)
lfs (LFS对象)
registry (注册图片)
pages (页面内容）

$ docker exec -it gitlab gitlab-rake gitlab:backup:create SKIP=artifacts 
```
### 恢复
```
chmod 666 /var/opt/gitlab/backups/${文件名称}_gitlab_backup.tar
$ docker exec -it gitlab gitlab-rake gitlab:backup:restore BACKUP=${文件名称}
```
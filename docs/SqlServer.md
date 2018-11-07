# SQLServer  
## 常用命令  
### 索引使用情况  
```
USE [DataBase]
SELECT
	objects.name						AS 表名,
	databases.name						AS 数据库名,
	indexes.name						AS 索引名,
	(
		CASE indexes.type
			WHEN 0 THEN N'堆'
			WHEN 1 THEN N'聚集'
			WHEN 2 THEN N'非聚集'
			WHEN 3 THEN N'XML'
			WHEN 4 THEN N'空间'
			WHEN 5 THEN N'聚集列存储索引。 适用范围： SQL Server 2014 (12.x) 到 SQL Server 2017。'
			WHEN 6 THEN N'非聚集列存储索引。 适用范围： SQL Server 2012 (11.x) 到 SQL Server 2017。'
			WHEN 7 THEN N'非聚集哈希索引。 适用范围： SQL Server 2014 (12.x) 到 SQL Server 2017。'
		END
	)									AS 索引的类型,
	partition_stats.row_count			AS 表行数,
	stats.user_seeks					AS 用户索引查找次数,
	stats.user_scans					AS 用户索引扫描次数,
	stats.user_updates					AS 用户索引更新次数,
	(
		CASE indexes.is_unique
			WHEN 1 THEN N'是'
			WHEN 0 THEN N'否'
		END
	)									AS 索引是否唯一,
	(
		CASE indexes.is_unique_constraint
			WHEN 1 THEN N'是'
			WHEN 0 THEN N'否'
		END
	)									AS [索引是否为主键约束的一部分],
	(
		CASE indexes.is_unique_constraint
			WHEN 1 THEN N'是'
			WHEN 0 THEN N'否'
		END
	)									AS [索引是否为唯一约束的一部分],
	(
		CASE indexes.is_disabled
			WHEN 1 THEN N'是'
			WHEN 0 THEN N'否'
		END
	)									AS [是否禁用索引],
	(
		CASE indexes.auto_created
			WHEN 1 THEN N'索引已通过自动优化'
			WHEN 0 THEN N'索引由用户创建'
		END
	)									AS [索引创建]
FROM
	sys.dm_db_index_usage_stats stats
	LEFT JOIN sys.objects objects ON stats.object_id = objects.object_id
	LEFT JOIN sys.databases databases ON databases.database_id = stats.database_id
	LEFT JOIN sys.indexes indexes ON indexes.index_id = stats.index_id 
	AND stats.object_id = indexes.object_id
	LEFT JOIN sys.dm_db_partition_stats partition_stats ON stats.object_id = partition_stats.object_id 
	AND indexes.index_id = partition_stats.index_id 
WHERE
	objects.name IS NOT NULL 
	AND indexes.name IS NOT NULL 
ORDER BY
	stats.object_id,
	indexes.index_id
```  
### 缺失索引  
```
USE [master]
SELECT
	mig.index_group_handle				AS 索引组
	, m.equality_columns				 							--构成相等谓词的列的逗号分隔列表，谓词的形式如下：table.column =constant_value
	, m.inequality_columns				 							--构成不等谓词的列的逗号分隔列表，例如以下形式的谓词：table.column > constant_value；“=”之外的任何比较运算符都表示不相等。
	, m.included_columns				 							--用于查询的涵盖列的逗号分隔列表。 有关涵盖列或包含列的详细信息，请参阅创建带有包含列的索引。对于内存优化索引（包括哈希和内存优化非聚集），请忽略 included_columns。 每个内存优化索引中均包含表的所有列。
	, m.[statement]						AS [索引缺失的表的名称]
	, mic.column_name					AS 表列的名称
	, mic.column_usage					AS 查询使用列的方式
	/*
	*	取值范围如下：
	*
	*	EQUALITY 列提供一个表示相等的谓词，其形式为：
	*	table.column = constant_value
	*
	*	INEQUALITY
	*	列包含表示不等的谓词，例如，如下形式的谓词：
	*	table.column > constant_value
	*	“=”之外的任何比较运算符都表示不相等。
	*	
	*	INCLUDE
	*	列不用于谓词赋值，但用于其他原因，例如包含一个查询。
	*
	*/
	, migs.unique_compiles				AS 收益编译次数				-- 将从该缺失索引组受益的编译和重新编译数。 许多不同查询的编译和重新编译可影响该列值。
	, migs.user_seeks					AS 收益查找次数				--由可能使用了组中建议索引的用户查询所导致的查找次数。
	, migs.user_scans					AS 收益扫描次数				--由可能使用了组中建议索引的用户查询所导致的扫描次数。
	, migs.last_user_seek				AS 最近收益查找时间			--由可能使用了组中建议索引的用户查询所导致的上次查找日期和时间。
	, migs.last_user_scan				AS 最近收益扫描时间			--由可能使用了组中建议索引的用户查询所导致的上次扫描日期和时间。
	, migs.avg_total_user_cost			AS 收益查询平均成本			--可通过组中的索引减少的用户查询的平均成本。
	, migs.avg_user_impact				AS 收益查询平均百分比		--实现此缺失索引组后，用户查询可能获得的平均百分比收益。 该值表示如果实现此缺失索引组，则查询成本将按此百分比平均下降。 
	, migs.system_seeks					AS 系统收益查找次数			--由可能使用了组中建议索引的系统查询（如自动统计信息查询）所导致的查找次数。 有关详细信息，请参阅Auto Stats 事件类。
	, migs.system_scans					AS 系统收益扫描次数			--由可能使用了组中建议索引的系统查询所导致的扫描次数。
	, migs.last_system_seek				AS 系统最近收益查找时间		--由可能使用了组中建议索引的系统查询所导致的上次系统查找日期和时间。
	, migs.last_system_scan				AS 系统最近收益扫描时间		--由可能使用了组中建议索引的系统查询所导致的上次系统扫描日期和时间。
	, migs.avg_total_system_cost		AS 系统收益查询平均成本		--可通过组中的索引减少的系统查询的平均成本。
	, migs.avg_system_impact			AS 系统收益查询平均百分比	--实现此缺失索引组后，系统查询可能获得的平均百分比收益。该值表示如果实现此缺失索引组，则查询成本将按此百分比平均下降。
FROM sys.dm_db_missing_index_details m
OUTER APPLY sys.dm_db_missing_index_columns(m.index_handle) mic
LEFT JOIN sys.dm_db_missing_index_groups mig ON mig.index_handle = m.index_handle
LEFT JOIN sys.dm_db_missing_index_group_stats migs ON migs.group_handle = mig.index_group_handle
ORDER BY migs.user_seeks DESC
	, migs.avg_total_user_cost DESC
	, mig.index_group_handle ASC
```  
### 实时SQL  
```
USE [master]
SELECT
	m.[session_id]							AS 会话的ID
	,m.[status]								AS 运行状态
	,m.[command]							AS 命令类型
	,m.[start_time]							AS 开始时间
	,DATEDIFF(
		ms,
		m.[start_time],
	GETDATE())								AS [运行时间（毫秒）]
	,DB_NAME( m.[database_id] )				AS 数据库
	,m.[wait_time]							AS [等待时间（毫秒）]
	,m.[wait_type]							AS 等待类型
	,m.[wait_resource]						AS 等待资源
	,CASE
		WHEN m.[wait_resource] LIKE 'OBJECT:%' THEN
		(
		SELECT
			tmp.name 
		FROM
			[api_center].sys.objects tmp 
		WHERE
			m.[wait_resource] LIKE N'%:' + CONVERT ( nvarchar ( MAX ), tmp.object_id ) + N':%' 
		) ELSE NULL 
	END										AS 等待资源 
	,m.[cpu_time]							AS [CPU耗时（毫秒）]
	,m.[logical_reads]						AS 逻辑读取数
	,( 
		SELECT tmp.text 
		FROM sys.dm_exec_sql_text ( m.[sql_handle] ) 
		tmp 
	)										AS [TSQL]
	,m.[last_wait_type]						AS 上次等待类型
	,m.[statement_start_offset]				AS 正在执行的语句开始位置的字符数
	,m.[statement_end_offset]				AS 指示当前正在执行的语句结束位置的字符数
	,m.[user_id]							AS 提交请求的用户的ID
	,m.[connection_id]						AS 请求到达时所采用的连接的ID
	,(
		CASE m.[blocking_session_id]
		WHEN -2 THEN N'阻塞资源由孤立的分布式事务拥有'
		WHEN -3 THEN N'阻塞资源由延迟的恢复事务拥有'
		WHEN -4 THEN N'由于内部闩锁状态转换而导致此时无法确定阻塞闩锁所有者的会话 ID'
		WHEN NULL THEN N'请求未被阻塞，或锁定会话的会话信息不可用（或无法进行标识）'
		WHEN 0 THEN N'请求未被阻塞，或锁定会话的会话信息不可用（或无法进行标识）'
		ELSE  N'未知'
		END
	) 										AS 正在阻塞请求的会话的ID
	,m.[open_transaction_count]				AS 打开的事务数
	,m.[open_resultset_count]				AS 打开结果集数
	,m.[transaction_id]						AS 事务的ID
	,m.[total_elapsed_time]					AS [请求到达后经过的总时间（毫秒）]
	,m.[reads]								AS 执行的读取数
	,m.[writes]								AS 执行的写入数
	,(
	CASE m.[transaction_isolation_level]
		WHEN 0 THEN N'未指定'
		WHEN 1 THEN N'未提交读取'
		WHEN 2 THEN N'已提交读取'
		WHEN 3 THEN N'可重复'
		WHEN 4 THEN N'可序列化'
		WHEN 5 THEN N'快照'
		ELSE NULL 
	END
	)	AS 事物隔离级别
	,m.[lock_timeout]						AS [锁定超时时间（毫秒）]
FROM
	sys.dm_exec_requests m 
WHERE
	m.[status] <> 'sleeping' 
	AND m.[status] <> 'background' --and m.[session_id] <> 133		--例外当前的会话id
	
ORDER BY
CASE
		
		WHEN m.[status] = 'suspended' THEN
		1 ELSE 0 
	END ASC,
	m.[status],
	m.[wait_type]
```  
### 检查活动连接  
```
USE [master]
sp_who2 active
```  
###

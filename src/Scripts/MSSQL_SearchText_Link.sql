select node.* from (
select stack_link.stack_id from (
SELECT stack_id, STRING_AGG([value],'') as [value] FROM [dbo].[stack_text] 	group by stack_id
) as d 
inner join stack on stack.id=d.stack_id
inner join stack_link on stack_link.node_id=stack.node_id
where d.[value] like '%{searchtext}%'
) as d1
inner join stack on stack.id=d1.stack_id
inner join node on node.id = stack.node_id

select node.* from (SELECT stack_id, group_concat(value SEPARATOR '') as v FROM stack_text group by `stack_id`) as d 
inner join stack on stack.id=d.stack_id
inner join node on node.id = stack.node_id
where d.v like '%{searchtext}%'
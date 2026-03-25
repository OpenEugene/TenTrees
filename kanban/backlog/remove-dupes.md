---
priority: medium
tags: []
---

# Remove dupes

Remove dupes with the following sql:

```sql
    SELECT AssessmentId,
           ROW_NUMBER() OVER (
               PARTITION BY GrowerId, AssessmentDate 
               ORDER BY AssessmentId
           ) AS rn
    FROM Assessment
    WHERE EnteredByAdmin = 1
)
DELETE FROM Dupes
WHERE rn > 1;
```

## Checklist

- [ ] Execute script

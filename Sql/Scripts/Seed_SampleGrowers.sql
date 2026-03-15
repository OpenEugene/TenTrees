/*
 * Seed Script: Sample Villages and Growers for Development / Testing
 *
 * Safe to re-run — each block checks for existence before inserting.
 * Villages are matched by VillageName; Growers are matched by GrowerName + VillageId.
 *
 * Status values (GrowerStatus enum):
 *   0 = Active
 *   1 = Inactive
 *   2 = Exited
 *
 * NOTE: MentorId stores an Oqtane UserId (string).
 *       Replace 'seed-mentor' with a real UserId from dbo.[User] after running.
 */

SET NOCOUNT ON;

DECLARE @Now       DATETIME2(7) = GETUTCDATE();
DECLARE @SeedUser  NVARCHAR(256) = 'seed';
DECLARE @MentorId  NVARCHAR(256) = 'seed-mentor';

-- ============================================================
-- 1. Villages
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM dbo.Village WHERE VillageName = 'Mahlori')
    INSERT INTO dbo.Village (VillageName, ContactName, ContactPhone, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES ('Mahlori', 'Joseph Maluleke', '082 111 2233', 1, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Village WHERE VillageName = 'Mthambothini')
    INSERT INTO dbo.Village (VillageName, ContactName, ContactPhone, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES ('Mthambothini', 'Anna Chauke', '082 444 5566', 1, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Village WHERE VillageName = 'Thulamahashe')
    INSERT INTO dbo.Village (VillageName, ContactName, ContactPhone, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES ('Thulamahashe', 'Simon Nkosi', '082 777 8899', 1, @SeedUser, @Now, @SeedUser, @Now);

DECLARE @V1 INT = (SELECT VillageId FROM dbo.Village WHERE VillageName = 'Mahlori');
DECLARE @V2 INT = (SELECT VillageId FROM dbo.Village WHERE VillageName = 'Mthambothini');
DECLARE @V3 INT = (SELECT VillageId FROM dbo.Village WHERE VillageName = 'Thulamahashe');

-- ============================================================
-- 2. Growers — Mahlori
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Mary Nkuna' AND VillageId = @V1)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V1, @MentorId, 'Mary Nkuna',      'A12', '8501015001089', '1985-01-01', 5, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Grace Maluleke' AND VillageId = @V1)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V1, @MentorId, 'Grace Maluleke',  'A07', '7203025002083', '1972-03-02', 3, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Agnes Sithole' AND VillageId = @V1)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V1, @MentorId, 'Agnes Sithole',   'B03', '6809145003081', '1968-09-14', 7, 0, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Peter Chauke' AND VillageId = @V1)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V1, @MentorId, 'Peter Chauke',    'C19', '9004085004082', '1990-04-08', 4, 0, 1, @SeedUser, @Now, @SeedUser, @Now);

-- ============================================================
-- 3. Growers — Mthambothini
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Tintswalo Mahlangu' AND VillageId = @V2)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V2, @MentorId, 'Tintswalo Mahlangu', 'D05', '8812205005084', '1988-12-20', 6, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Joyce Ndlovu' AND VillageId = @V2)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V2, @MentorId, 'Joyce Ndlovu',    'D11', '7506155006086', '1975-06-15', 4, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Samuel Mthethwa' AND VillageId = @V2)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V2, @MentorId, 'Samuel Mthethwa', 'E02', '6301225007081', '1963-01-22', 8, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Nomsa Khoza' AND VillageId = @V2)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V2, @MentorId, 'Nomsa Khoza',     'E08', '9210065008083', '1992-10-06', 2, 0, 2, @SeedUser, @Now, @SeedUser, @Now);

UPDATE dbo.Grower
SET    ExitDate   = DATEADD(MONTH, -3, @Now),
       ExitReason = 'Relocated',
       ExitNotes  = 'Family moved to Johannesburg (seed data)',
       ModifiedBy = @SeedUser,
       ModifiedOn = @Now
WHERE  GrowerName = 'Nomsa Khoza'
  AND  VillageId  = @V2
  AND  Status     = 2
  AND  ExitDate   IS NULL;

-- ============================================================
-- 4. Growers — Thulamahashe
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Sarah Nkosi' AND VillageId = @V3)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V3, @MentorId, 'Sarah Nkosi',     'F01', '8007145009087', '1980-07-14', 5, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Moses Nzuza' AND VillageId = @V3)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V3, @MentorId, 'Moses Nzuza',     'F14', '7811085010085', '1978-11-08', 6, 0, 0, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'Anna Chauke' AND VillageId = @V3)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V3, @MentorId, 'Anna Chauke',     'G09', '9503085011082', '1995-03-08', 3, 1, 1, @SeedUser, @Now, @SeedUser, @Now);

IF NOT EXISTS (SELECT 1 FROM dbo.Grower WHERE GrowerName = 'David Maluleke' AND VillageId = @V3)
    INSERT INTO dbo.Grower (VillageId, MentorId, GrowerName, HouseNumber, IdNumber, BirthDate, HouseholdSize, OwnsHome, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
    VALUES (@V3, @MentorId, 'David Maluleke',  'G22', '8405165012089', '1984-05-16', 4, 1, 0, @SeedUser, @Now, @SeedUser, @Now);

-- ============================================================
-- Summary
-- ============================================================

SELECT
    v.VillageName,
    COUNT(*)                                              AS TotalGrowers,
    SUM(CASE WHEN g.Status = 0 THEN 1 ELSE 0 END)        AS Active,
    SUM(CASE WHEN g.Status = 1 THEN 1 ELSE 0 END)        AS Inactive,
    SUM(CASE WHEN g.Status = 2 THEN 1 ELSE 0 END)        AS Exited
FROM dbo.Grower  g
JOIN dbo.Village v ON g.VillageId = v.VillageId
WHERE v.VillageName IN ('Mahlori', 'Mthambothini', 'Thulamahashe')
GROUP BY v.VillageName
ORDER BY v.VillageName;

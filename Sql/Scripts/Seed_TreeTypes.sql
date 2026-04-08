-- Seed 2026 tree list (43 items across 5 categories)
-- Run after TreeType table creation

SET IDENTITY_INSERT [dbo].[TreeType] ON;

INSERT INTO [dbo].[TreeType] ([TreeTypeId], [Name], [Category], [SortOrder], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES
    -- Medicinal Trees
    (1,  'Paper bark',      'Medicinal Trees', 1,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (2,  'Spek boom',       'Medicinal Trees', 2,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (3,  'Wild plum',       'Medicinal Trees', 3,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (4,  'Moringa',         'Medicinal Trees', 4,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (5,  'Longtail cassia', 'Medicinal Trees', 5,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (6,  'Kei Apple',       'Medicinal Trees', 6,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (7,  'Camphor bush',    'Medicinal Trees', 7,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (8,  'Cancer bush',     'Medicinal Trees', 8,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (9,  'Sour sop',        'Medicinal Trees', 9,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),

    -- Fruity Medicinal Trees
    (10, 'Avocado',         'Fruity Medicinal Trees', 1,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (11, 'Banana',          'Fruity Medicinal Trees', 2,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (12, 'Granadilla',      'Fruity Medicinal Trees', 3,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (13, 'Grapefruit',      'Fruity Medicinal Trees', 4,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (14, 'Lemon',           'Fruity Medicinal Trees', 5,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (15, 'Litchi',          'Fruity Medicinal Trees', 6,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (16, 'Loquat',          'Fruity Medicinal Trees', 7,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (17, 'Gooseberry',      'Fruity Medicinal Trees', 8,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (18, 'Mango',           'Fruity Medicinal Trees', 9,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (19, 'Naartjie',        'Fruity Medicinal Trees', 10, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (20, 'Orange',          'Fruity Medicinal Trees', 11, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (21, 'Paw paw',         'Fruity Medicinal Trees', 12, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (22, 'Pomegranate',     'Fruity Medicinal Trees', 13, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (23, 'Shaddock',        'Fruity Medicinal Trees', 14, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (24, 'Java plum',       'Fruity Medicinal Trees', 15, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),

    -- Nut Trees
    (25, 'Macadamia',       'Nut Trees', 1, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (26, 'Pecan',           'Nut Trees', 2, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (27, 'Cashew nut',      'Nut Trees', 3, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),

    -- Bushy, Herb & Medicinal Plants
    (28, 'Ginger',          'Bushy, Herb & Medicinal Plants', 1,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (29, 'Turmeric',        'Bushy, Herb & Medicinal Plants', 2,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (30, 'Garlic chives',   'Bushy, Herb & Medicinal Plants', 3,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (31, 'Thyme',           'Bushy, Herb & Medicinal Plants', 4,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (32, 'Wormwood',        'Bushy, Herb & Medicinal Plants', 5,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (33, 'Lemon verbena',   'Bushy, Herb & Medicinal Plants', 6,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (34, 'Yarrow',          'Bushy, Herb & Medicinal Plants', 7,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (35, 'Rosemary',        'Bushy, Herb & Medicinal Plants', 8,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (36, 'Rose geranium',   'Bushy, Herb & Medicinal Plants', 9,  1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (37, 'Curry plant',     'Bushy, Herb & Medicinal Plants', 10, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (38, 'Spearmint',       'Bushy, Herb & Medicinal Plants', 11, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (39, 'Basil',           'Bushy, Herb & Medicinal Plants', 12, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (40, 'Lemon grass',     'Bushy, Herb & Medicinal Plants', 13, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (41, 'Comfrey',         'Bushy, Herb & Medicinal Plants', 14, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),
    (42, 'Lavender',        'Bushy, Herb & Medicinal Plants', 15, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE()),

    -- Vegetable Medicinal Plants
    (43, 'Chaya spinach',   'Vegetable Medicinal Plants', 1, 1, 'system', GETUTCDATE(), 'system', GETUTCDATE());

SET IDENTITY_INSERT [dbo].[TreeType] OFF;

Feature: Garden Monitoring

Scenario: Record successful monitoring visit
  Given I am assigned to monitor "Peter's Garden"
  When I visit the garden and access the monitoring form
  And I enter visit date as today's date
  And I enter "10" trees planted
  And I enter "9" trees still alive
  And I select "Yes" for trees looking healthy
  And I select "No" for chemical fertilizers usage
  And I select "No" for pesticides usage
  And I select "Yes" for trees being mulched
  And I select "Yes" for compost making
  And I select "Yes" for water collection
  And I add a photo of the garden
  And I submit the monitoring report
  Then the monitoring record should be saved
  And the garden's progress should be updated
  And alerts should be generated if intervention is needed

Scenario: Report garden problems
  Given I am conducting a monitoring visit
  When I notice trees have broken branches
  And I select "The trees have broken branches" from the problems list
  And I add notes "Several branches broken by wind, need pruning support"
  And I submit the report
  Then a problem ticket should be created
  And the garden should be flagged for follow-up
  And appropriate staff should be notified

Scenario: Track tree mortality
  Given there are 10 trees planted in the garden
  When I visit and find only 7 trees alive
  And I enter "7" for trees still alive
  And I enter "Wind damage" as the reason for dead trees
  Then the system should calculate 30% mortality rate
  And if mortality exceeds threshold, an alert should be generated
  And replacement tree recommendations should be suggested
Feature: Site Assessment

Scenario: Complete site mapping assessment
  Given I am visiting an approved applicant's garden
  When I access the site assessment form on my mobile device
  And I enter site name "Peter's Garden"
  And I select "Yes" for water availability
  And I select "No" for water catchment system
  And I enter "5" for existing trees count
  And I enter "3" for indigenous trees count
  And I enter "2" for fruit/nut trees count
  And I select "Yes" for space for more trees
  And I select "Yes" for property fencing
  And I select "Yes" for compost resources
  And I submit the assessment
  Then the garden record should be created
  And the site should be ready for tree planting scheduling

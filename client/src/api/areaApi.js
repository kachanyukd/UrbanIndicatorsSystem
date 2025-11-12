export const getAreas = async () => {
  return [
    { id: 1, name: "Downtown" },
    { id: 2, name: "Old Town" },
    { id: 3, name: "Industrial Zone" },
  ];
};

export const getAreaDetails = async (id) => {
  return {
    id,
    name: `Area ${id}`,
    description: "Тестовий район для демонстрації API.",
  };
};

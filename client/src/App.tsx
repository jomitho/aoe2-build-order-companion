import { useEffect, useState } from 'react';
import "./App.css";
import { getBuildOrders } from "./api/buildOrdersApi";
import type { BuildOrderSummary } from "./types/buildOrder";

function App() {
  const [buildOrders, setBuildOrders] = useState<BuildOrderSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    async function loadBuildOrders() {
      try {
        const data = await getBuildOrders();
        setBuildOrders(data);
      } catch {
        setErrorMessage("Could not load build orders. Is the API running?");
      } finally {
        setIsLoading(false);
      }
    }

    loadBuildOrders();
  }, []);

  return (
    <main className="app">
      <section className="hero">
        <p className="eyebrow">Age of Empires II</p>
        <h1>Build Order Companion</h1>
        <p className="heroText">
          Create, manage and follow build orders for Age of Empires II.
        </p>
      </section>

      <section className="content">
        <div className="sectionHeader">
          <h2>Build orders</h2>
          <span>{buildOrders.length} available</span>
        </div>

        {isLoading && <p>Loading build orders...</p>}

        {errorMessage && <p className="error">{errorMessage}</p>}

        {!isLoading && !errorMessage && (
          <div className="buildOrderGrid">
            {buildOrders.map((buildOrder) => (
              <article key={buildOrder.id} className="buildOrderCard">
                <div className="cardHeader">
                  <h3>{buildOrder.name}</h3>
                  <span className="difficulty">{buildOrder.difficulty}</span>
                </div>

                <p className="description">{buildOrder.description}</p>

                <dl className="metadata">
                  <div>
                    <dt>Civilization</dt>
                    <dd>{buildOrder.civilization}</dd>
                  </div>
                  <div>
                    <dt>Strategy</dt>
                    <dd>{buildOrder.strategyType}</dd>
                  </div>
                  <div>
                    <dt>Steps</dt>
                    <dd>{buildOrder.stepCount}</dd>
                  </div>
                </dl>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  );
}

export default App
